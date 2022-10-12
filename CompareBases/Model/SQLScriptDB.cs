using CompareBases.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareBases
{
    public class SQLScriptDB : ISqlScript
	{
		public bool WithTable { get; set; }
		public bool TableWithTrigger { get; set; }
		public string FilterPrefix { get; set; }
		public List<string> FilterIgnoreByPrefix { get; set; }
		public List<string> FilterIgnoreByPostfix { get; set; }

        public string GetScriptCountObjects()
        {
            return GetScriptPrepareWhere() + @"
select count(1)
from sys.objects so 
	inner join sys.schemas ss on so.schema_id = ss.schema_id
	--left join sys.sql_modules sm on sm.object_id = so.object_id
" + GetScriptWhere();
        }

        private string GetScriptSchemas(bool useFilterPrefix = true)
        {
            return @"   
union all
select -schema_id object_id, ' ' [type], name schemaName, '' name, 'CREATE SCHEMA [' + name + ']' definition, getdate() create_date, getdate() modify_date
from sys.schemas s 
    "
                + (!useFilterPrefix || string.IsNullOrWhiteSpace(FilterPrefix) ? "" : " where '[' + name + ']' like '"
                    + GetStringLike(FilterPrefix)
                    + "%'");
        }

        private string GetScriptWhere(string types = null, bool useFilterPrefix = true)
        {
           if (types == null) types = WithTable ? "'P ', 'TF', 'FN', 'IF', 'V ', 'U '" : "'P ', 'TF', 'FN', 'IF', 'V '";
            return @"
	cross apply (select '[' + ss.name + '].[' + so.name  + ']' fName) rr
    left join @filterIgnore fi on rr.fName like fi.mask
where so.[type] in (" + types + @")
    and fi.mask is null
"
                + (!useFilterPrefix || string.IsNullOrWhiteSpace(FilterPrefix) ? "" : " and rr.fName like '"
                    + GetStringLike(FilterPrefix)
                    + "%'")
                ;
        }

        private string GetStringLike(string orig)
        {
            return orig.Replace("'", "''").Replace("[", "[[]").Replace("_", "[_]");
        }

        private string GetScriptPrepareWhere()
        {
            return @"
declare @filterIgnore table (mask varchar(max))
"
                + FilterIgnoreByPrefix.Where(f => !string.IsNullOrWhiteSpace(f)).Aggregate(""
                    , (s, f) => (s == "" ? "insert @filterIgnore(mask) " : s + Environment.NewLine + "union ")
                        + "select '" + GetStringLike(f) + "%'")
                + Environment.NewLine
                + FilterIgnoreByPostfix.Where(f => !string.IsNullOrWhiteSpace(f)).Aggregate(""
                    , (s, f) => (s == "" ? "insert @filterIgnore(mask) " : s + Environment.NewLine + "union ")
                        + "select '%" + GetStringLike(f) + "'")
                + Environment.NewLine;
        }

        public string GetScriptDefinitionObjects()
        {
            if (!WithTable)
                return GetScriptPrepareWhere() + @"
select so.object_id, so.[type], ss.name schemaName, so.name, sm.definition, so.create_date, so.modify_date
from sys.objects so 
	inner join sys.schemas ss on so.schema_id = ss.schema_id
	left join sys.sql_modules sm on sm.object_id = so.object_id
" + GetScriptWhere() + GetScriptSchemas();

            var scr = GetScriptPrepareWhere() + @"
select so.object_id, so.[type], ss.name schemaName, so.name, convert(varchar(max), null) definition, so.create_date, so.modify_date
into #resultDef 
from sys.objects so 
	inner join sys.schemas ss on so.schema_id = ss.schema_id
	--left join sys.sql_modules sm on sm.object_id = so.object_id
" + GetScriptWhere();
            //скрипт содания таблицы (колонки)
            #region скрипт содания таблицы
            scr += @"

create clustered index IX_#resultDef on #resultDef(object_id)

select so.object_id table_object_id
	, rr.fName
	, sc.name resCol
	, '[' + sc.name + '] [' +
		st.name + ']' +
		case 
			when st.name = 'numeric' or st.name = 'decimal' 
			then ' (' + cast(sc.xprec as varchar(20)) + ',' + cast(sc.xscale as varchar(20)) + ')'
			when st.name like '%char'
			then ' (' + (case when sc.[length] = -1 then 'max' else cast((sc.[length] / (case when st.name like 'n%char' then 2 else 1 end)) as varchar(20)) end) + ')'
			else ''
		end + ' ' +
		case when sc.isnullable = 1 then 'NULL' else 'NOT NULL' end +
		case when idc.column_id is not null then ' IDENTITY(' + cast(ident_seed(rr.fName) as varchar(30)) + ', ' + cast(ident_incr(rr.fName) as varchar(30)) + ')' else '' end
		resType
	, cast(rank() over (partition by r.object_id order by (case when idc.column_id is not null then 0 else 1 end), sc.name) as int) num
into #resultDefTabCols
from #resultDef r
	cross apply (select '[' + r.schemaName + '].[' + r.name  + ']' fName) rr
	inner join sys.objects so on so.object_id = r.object_id
	inner join dbo.syscolumns sc on so.object_id = sc.id
	inner join dbo.systypes st on sc.xtype = st.xtype and sc.xusertype=st.xusertype
	left join sys.identity_columns idc on idc.object_id = so.object_id and idc.column_id = sc.colid
where r.[type] = 'U ' 

create clustered index IX_#resultDefTabCols on #resultDefTabCols(table_object_id, num)

;with t
as
(
	select table_object_id, max(fName) fName
		, cast(0 as int) num
		, count(1) numMax
		, convert(varchar(max), 'CREATE TABLE ' + max(fName) + ' 
( 
') [definition]
	from #resultDefTabCols
	group by table_object_id
	
	union all
	
	select t.table_object_id, t.fName, tc.num, t.numMax
		, convert(varchar(max), t.[definition] + (case when t.num > 0 then ', 
' else '' end) + tc.resType)
	from t
		inner join #resultDefTabCols tc on tc.table_object_id = t.table_object_id and tc.num = t.num + 1
)
select table_object_id, fName tableName, [definition]+ ' 
)' [definition]
into #resultDefTab
from t
where numMax = num
option (maxrecursion 500)

create clustered index IX_#resultDefTab on #resultDefTab(table_object_id)
";
            #endregion

            //ограничения
            #region ограничения
            scr += @"
declare @table_object_id bigint, @tableName varchar(max)

declare constr_cnst cursor local static for
select table_object_id, tableName from #resultDefTab
for read only

open constr_cnst
fetch constr_cnst into @table_object_id, @tableName
while @@fetch_status >= 0
begin
	
	declare @objname nvarchar(776) set @objname = @tableName
	
"
                + SQLSrc_sp_helpconstraint
                + @"

	declare @resultDefTabConstr varchar(max), @rdtcix int
    set @resultDefTabConstr = ''
	select @rdtcix = @rdtcix + row_number() over (order by (case when cnst_2type = 'PK' then 0 else 1 end), cnst_nonblank_name)
		, @resultDefTabConstr = @resultDefTabConstr + '

'
		+ case when cnst_2type = 'D ' then
			'ALTER TABLE ' + @objname + ' ADD CONSTRAINT ' + cnst_nonblank_name + ' ' + replace(cnst_type, 'on column', cnst_keys + ' FOR ')
			
			when cnst_2type = 'PK' then
			'ALTER TABLE ' + @objname + ' ADD CONSTRAINT ' + cnst_nonblank_name + ' ' + replace(replace(cnst_type, '(clustered)', 'CLUSTERED'), '(non-clustered)', 'NONCLUSTERED')
				+ ' (' + cnst_keys + ')'
			
			when cnst_2type = 'UQ' then
			'ALTER TABLE ' + @objname + ' ADD CONSTRAINT ' + cnst_nonblank_name + ' ' + replace(replace(cnst_type, '(clustered)', 'CLUSTERED'), '(non-clustered)', 'NONCLUSTERED')
				+ ' (' + cnst_keys + ')'
			
			when cnst_2type = 'F ' then
			'ALTER TABLE ' + @objname + ' ADD CONSTRAINT ' + cnst_nonblank_name + ' ' + cnst_type + ' (' + cnst_keys + ') ' + replace(cnst_keys2, db_name() +'.', '')
				+ (case when cnst_delcasc = 1 then ' ON DELETE CASCADE' else '' end)
				+ (case when cnst_updcasc = 1 then ' ON UPDATE CASCADE' else '' end)
			
			when cnst_2type = 'C ' then
			'ALTER TABLE ' + @objname + ' ADD CONSTRAINT ' + cnst_nonblank_name + ' CHECK ' + cnst_keys
			
			end
	from (	select cnst_id, max(cnst_type) cnst_type, max(cnst_name) cnst_name, max(cnst_nonblank_name) cnst_nonblank_name, max(cnst_2type) cnst_2type
				, max(convert(int, cnst_disabled)) cnst_disabled, max(convert(int, cnst_notrepl)) cnst_notrepl, max(convert(int, cnst_delcasc)) cnst_delcasc, max(convert(int, cnst_updcasc)) cnst_updcasc
				, max(case when cnst_type <> ' ' then cnst_keys end) cnst_keys
				, max(case when cnst_type = ' ' then cnst_keys end) cnst_keys2
			from #spcnsttab
			group by cnst_id
		) t
	--order by (case when cnst_2type = 'PK' then 0 else 1 end), cnst_nonblank_name
	
	update #resultDefTab
	set [definition] = [definition] + @resultDefTabConstr
	where table_object_id = @table_object_id
	
	fetch constr_cnst into @table_object_id, @tableName
end
deallocate constr_cnst

";
            #endregion 

            //индексы
            #region индексы
            scr += @"

--	drop table #resultDefIndexCol
SELECT r.table_object_id
	, ind.object_id
	, ind.name indexName
	, cast(rank() over (partition by ind.object_id, ind.name 
        order by (case when ic.is_included_column = 1 then col.name else '' end), ic.is_descending_key, ic.index_column_id) as int) num
	, cast(ic.index_id as int) isClustered 
	, cast(ind.is_unique as int) isUnique
	, ic.index_column_id
	, ic.is_included_column
	, ic.is_descending_key
	, col.name columnName
into #resultDefIndexCol
FROM #resultDefTab r
inner join sys.indexes ind on ind.object_id = r.table_object_id
INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
INNER JOIN sys.tables t ON ind.object_id = t.object_id 
WHERE ind.is_primary_key = 0 AND ind.is_unique_constraint = 0 AND t.is_ms_shipped = 0 
   
create clustered index IX_#resultDefIndexCol on #resultDefIndexCol(object_id, indexName, num)

--	drop table #resultDefIndex  
;with t
as
(
	select max(table_object_id) table_object_id, object_id, indexName, max(obj.objName) objName
		, cast(0 as int) num
		, cast(0 as int) isInclude
		, count(1) numMax
		, cast('CREATE ' 
			+ (case when max(isUnique) = 1 then 'UNIQUE' else '' end) 
			+ (case when max(isClustered) = 1 then ' CLUSTERED' else ' NONCLUSTERED' end) 
			+ ' INDEX ' + indexName + ' ON ' + max(obj.objName) + ' (' as varchar(max)) res
	from #resultDefIndexCol
		cross apply (select '[' + object_schema_name(table_object_id) + '].[' + object_name(table_object_id) + ']' objName) obj
	group by object_id, indexName
	
	union all 
	
	select t.table_object_id, t.object_id, t.indexName, t.objName, cast(ci.num as int) num
		, cast(ci.is_included_column as int) isInclude
		, t.numMax
		, cast(t.res 
			+ (case when ci.is_included_column <> t.isInclude then ') INCLUDE (' when t.num <> 0 then ', ' else '' end)
			+ ci.columnName + (case when ci.is_descending_key = 1 then ' desc' else '' end)
			+ '' as varchar(max)) res
	from t
		inner join #resultDefIndexCol ci on ci.object_id = t.object_id and ci.indexName = t.indexName and ci.num = t.num + 1
)
select table_object_id, res + ')' [definition] 
	, cast(rank() over (partition by table_object_id order by res) as int) num
into #resultDefIndex
from t
where numMax = num

create clustered index IX_#resultDefIndex on #resultDefIndex(table_object_id, num)

;with t
as
(
	select table_object_id
		, cast(1 as int) num
		, convert(varchar(max), '

' + [definition]) [definition]
	from #resultDefIndex
	where num = 1
	
	union all
	
	select t.table_object_id
		, cast(n.num as int) num
		, convert(varchar(max), t.[definition] + '

' + n.[definition]) [definition]
	from t
		inner join #resultDefIndex n on n.table_object_id = t.table_object_id and n.num = t.num + 1
)
update r 
set [definition] = r.[definition] + t.[definition]
from #resultDefTab r 
	inner join t on r.table_object_id = t.table_object_id
where t.num = (select max(num) from #resultDefIndex tt where tt.table_object_id = t.table_object_id)
option (maxrecursion 500)

";
            #endregion

            //триггеры
            #region триггеры
            if (TableWithTrigger)
                scr += @"

select so.parent_object_id table_object_id, so.object_id, sm.[definition]
	, cast(rank() over (partition by parent_object_id order by so.name) as int) num
into #resultDefTrig
from #resultDef r
	inner join sys.objects so on so.parent_object_id = r.object_id
	inner join sys.schemas ss on so.schema_id = ss.schema_id
	inner join sys.sql_modules sm on sm.object_id = so.object_id
" + GetScriptWhere("'TR'", false) + @"

create clustered index IX_#resultDefTrig on #resultDefTrig(table_object_id, num)

;with t
as
(
	select table_object_id, num, convert(varchar(max), '

GO
' + [definition]) [definition]
	from #resultDefTrig
	where num = 1
	
	union all 
	
	select t.table_object_id, rt.num, convert(varchar(max), t.[definition] + '

GO
' + rt.[definition])
	from t
		inner join #resultDefTrig rt on rt.table_object_id = t.table_object_id and rt.num = t.num + 1
)
update r 
set [definition] = r.[definition] + t.[definition]
from #resultDefTab r 
	inner join t on r.table_object_id = t.table_object_id
where t.num = (select max(num) from #resultDefTrig tt where tt.table_object_id = t.table_object_id)

";
            #endregion

            //вывод
            #region вывод
            scr += @"

select r.[object_id]
     , r.[type]
     , r.schemaName
     , r.name
     , isnull(sm.[definition], t.[definition]) [definition]
     , r.create_date
     , r.modify_date
from #resultDef r
    left join sys.sql_modules sm on sm.object_id = r.object_id
    left join #resultDefTab t on r.object_id = t.table_object_id

" + GetScriptSchemas();
            #endregion

            return scr;
        }

        #region Кусок процедуры sys.sp_helpconstraint без вывода. Перед ним  declare @objname nvarchar(776) set @objname = ''

        private const string SQLSrc_sp_helpconstraint = @"

	if object_id('tempdb..#spcnsttab') is not null drop table #spcnsttab

	declare	@objid			int           -- the object id of the table
			,@cnstdes		nvarchar(4000)-- string to build up index desc
			,@cnstname		sysname       -- name of const. currently under consideration
			,@i				int
			,@cnstid		int
			,@cnsttype		character(2)
			,@keys			nvarchar(2126)	--Length (16*max_identifierLength)+(15*2)+(16*3)
			,@dbname		sysname

	-- Create temp table
	CREATE TABLE #spcnsttab
	(
		cnst_id			int			NOT NULL
		,cnst_type			nvarchar(146) collate database_default NOT NULL   -- 128 for name + text for DEFAULT
		,cnst_name			sysname		collate database_default NOT NULL
		,cnst_nonblank_name	sysname		collate database_default NOT NULL
		,cnst_2type			character(2)	collate database_default NULL
		,cnst_disabled		bit				NULL
		,cnst_notrepl		bit				NULL
		,cnst_delcasc		bit				NULL
		,cnst_updcasc		bit				NULL
		,cnst_keys			nvarchar(2126)	collate database_default NULL	-- see @keys above for length descr
	)

	-- Check to see that the object names are local to the current database.
	select @dbname = parsename(@objname,3)

	if @dbname is null
		select @dbname = db_name()
	else if @dbname <> db_name()
		begin
			raiserror(15250,-1,-1)
		end

	-- Check to see if the table exists and initialize @objid.
	select @objid = object_id(@objname)
	if @objid is NULL
	begin
		raiserror(15009,-1,-1,@objname,@dbname)
	end

	-- STATIC CURSOR OVER THE TABLE'S CONSTRAINTS
	declare ms_crs_cnst cursor local static for
		select object_id, type, name from sys.objects where parent_object_id = @objid
			and type in ('C ','PK','UQ','F ', 'D ')	-- ONLY 6.5 sysconstraints objects
		for read only

	-- Now check out each constraint, figure out its type and keys and
	-- save the info in a temporary table that we'll print out at the end.
	open ms_crs_cnst
	fetch ms_crs_cnst into @cnstid ,@cnsttype ,@cnstname
	while @@fetch_status >= 0
	begin

		if @cnsttype in ('PK','UQ')
		begin
			-- get indid and index description
			declare @indid smallint
			select	@indid = index_id,
					@cnstdes = case when @cnsttype = 'PK'
								then 'PRIMARY KEY' else 'UNIQUE' end
							 + case when index_id = 1
								then ' (clustered)' else ' (non-clustered)' end
			from		sys.indexes
			where	object_id = @objid and name = object_name(@cnstid)

			-- Format keys string
			declare @thiskey nvarchar(131) -- 128+3

			select @keys = index_col(@objname, @indid, 1), @i = 2
			if (indexkey_property(@objid, @indid, 1, 'isdescending') = 1)
				select @keys = @keys  + '(-)'

			select @thiskey = index_col(@objname, @indid, @i)
			if ((@thiskey is not null) and (indexkey_property(@objid, @indid, @i, 'isdescending') = 1))
				select @thiskey = @thiskey + '(-)'

			while (@thiskey is not null)
			begin
				select @keys = @keys + ', ' + @thiskey, @i = @i + 1
				select @thiskey = index_col(@objname, @indid, @i)
				if ((@thiskey is not null) and (indexkey_property(@objid, @indid, @i, 'isdescending') = 1))
					select @thiskey = @thiskey + '(-)'
			end

			-- ADD TO TABLE
			insert into #spcnsttab
				(cnst_id,cnst_type,cnst_name, cnst_nonblank_name,cnst_keys, cnst_2type)
			values (@cnstid, @cnstdes, @cnstname, @cnstname, @keys, @cnsttype)
		end

		else
		if @cnsttype = 'F '
		begin
			-- OBTAIN TWO TABLE IDs
			declare @fkeyid int, @rkeyid int
			select @fkeyid = parent_object_id, @rkeyid = referenced_object_id
				from sys.foreign_keys where object_id = @cnstid

			-- USE CURSOR OVER FOREIGN KEY COLUMNS TO BUILD COLUMN LISTS
			--	(NOTE: @keys HAS THE FKEY AND @cnstdes HAS THE RKEY COLUMN LIST)
			declare ms_crs_fkey cursor local for
				select parent_column_id, referenced_column_id
					from sys.foreign_key_columns where constraint_object_id = @cnstid
			open ms_crs_fkey
			declare @fkeycol smallint, @rkeycol smallint
			fetch ms_crs_fkey into @fkeycol, @rkeycol
			select @keys = col_name(@fkeyid, @fkeycol), @cnstdes = col_name(@rkeyid, @rkeycol)
			fetch ms_crs_fkey into @fkeycol, @rkeycol
			while @@fetch_status >= 0
			begin
				select	@keys = @keys + ', ' + col_name(@fkeyid, @fkeycol),
						@cnstdes = @cnstdes + ', ' + col_name(@rkeyid, @rkeycol)
				fetch ms_crs_fkey into @fkeycol, @rkeycol
			end
			deallocate ms_crs_fkey

			-- ADD ROWS FOR BOTH SIDES OF FOREIGN KEY
			insert into #spcnsttab
				(cnst_id, cnst_type,cnst_name,cnst_nonblank_name,
					cnst_keys, cnst_disabled,
					cnst_notrepl, cnst_delcasc, cnst_updcasc, cnst_2type)
			values
				(@cnstid, 'FOREIGN KEY', @cnstname, @cnstname,
					@keys, ObjectProperty(@cnstid, 'CnstIsDisabled'),
					ObjectProperty(@cnstid, 'CnstIsNotRepl'),
					ObjectProperty(@cnstid, 'CnstIsDeleteCascade'),
					ObjectProperty(@cnstid, 'CnstIsUpdateCascade'),
					@cnsttype)
			insert into #spcnsttab
				(cnst_id,cnst_type,cnst_name,cnst_nonblank_name,
					cnst_keys,
					cnst_2type)
			select
				@cnstid,' ', ' ', @cnstname,
					'REFERENCES ' + db_name()
						+ '.' + rtrim(schema_name(ObjectProperty(@rkeyid,'schemaid')))
						+ '.' + object_name(@rkeyid) + ' ('+@cnstdes + ')',
					@cnsttype
		end

		else
		if @cnsttype = 'C'
		begin
			-- Check constraint
			select @i = 1
			select @cnstdes = null
			select @cnstdes = text from syscomments where id = @cnstid and colid = @i

			insert into	#spcnsttab
				(cnst_id, cnst_type ,cnst_name ,cnst_nonblank_name,
					cnst_keys, cnst_disabled, cnst_notrepl, cnst_2type)
			select	@cnstid,
				case when parent_column_id <> 0
					then 'CHECK on column ' + col_name(@objid, parent_column_id)
					else 'CHECK Table Level ' end,
				@cnstname ,@cnstname ,substring(@cnstdes,1,2000),
				is_disabled, is_not_for_replication,
				@cnsttype
			from sys.check_constraints where object_id = @cnstid

			while @cnstdes is not null
			begin
				if @i > 1
					insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
					select	@cnstid,' ' ,' ' ,@cnstname ,substring(@cnstdes,1,2000), @cnsttype

				if len(@cnstdes) > 2000
					insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
					select	@cnstid,' ' ,' ' ,@cnstname ,substring(@cnstdes,2001,2000), @cnsttype

				select @i = @i + 1
				select @cnstdes = null
				select @cnstdes = text from syscomments where id = @cnstid and colid = @i
			end
		end

		else
		if (@cnsttype = 'D')
		begin
			select @i = 1
			select @cnstdes = null
			select @cnstdes = text from syscomments where id = @cnstid and colid = @i
			insert into	#spcnsttab
				(cnst_id,cnst_type ,cnst_name ,cnst_nonblank_name ,cnst_keys, cnst_2type)
			select @cnstid, 'DEFAULT on column ' + col_name(@objid, parent_column_id),
				@cnstname ,@cnstname ,substring(@cnstdes,1,2000), @cnsttype
				from sys.default_constraints where object_id = @cnstid

			while @cnstdes is not null
			begin
				if @i > 1
					insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
					select	@cnstid,' ' ,' ' ,@cnstname ,substring(@cnstdes,1,2000), @cnsttype

				if len(@cnstdes) > 2000
					insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
					select	@cnstid,' ' ,' ' ,@cnstname ,substring(@cnstdes,2001,2000), @cnsttype

				select @i = @i + 1
				select @cnstdes = null
				select @cnstdes = text from syscomments where id = @cnstid and colid = @i
			end
		end

		fetch ms_crs_cnst into @cnstid ,@cnsttype ,@cnstname
	end		--of major loop
	deallocate ms_crs_cnst

	-- Find any rules or defaults bound by the sp_bind... method.
/*
	insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
	select c.rule_object_id,'RULE on column ' + c.name + ' (bound with sp_bindrule)',
		object_name(c.rule_object_id), object_name(c.rule_object_id), m.text, 'R '
	from	sys.columns c join syscomments m on m.id = c.rule_object_id
	where c.object_id = @objid

	insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
	select c.default_object_id, 'DEFAULT on column ' + c.name + ' (bound with sp_bindefault)',
		object_name(c.default_object_id),object_name(c.default_object_id), m.text, 'D '
	from	sys.columns c join syscomments m on m.id = c.default_object_id
	where c.object_id = @objid and objectproperty(c.default_object_id, 'IsConstraint') = 0
*/
	if object_id(N'tempdb..#spcnsttabCashRULE', N'U') is null
	begin
		
		select c.object_id, c.rule_object_id cnst_id,'RULE on column ' + c.name + ' (bound with sp_bindrule)' cnst_type,
			object_name(c.rule_object_id) cnst_name, object_name(c.rule_object_id) cnst_nonblank_name, m.text cnst_keys, 'R ' cnst_2type
		into #spcnsttabCashRULE
		from	sys.columns c join syscomments m on m.id = c.rule_object_id
		
		create clustered index IX_#spcnsttabCashRULE on #spcnsttabCashRULE(object_id)
	end

	insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
	select cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type
	from #spcnsttabCashRULE
	where object_id = @objid
	
	if object_id(N'tempdb..#spcnsttabCashDEFAULT', N'U') is null
	begin
		
		select c.object_id, c.default_object_id cnst_id,'DEFAULT on column ' + c.name + ' (bound with sp_bindefault)' cnst_type,
			object_name(c.default_object_id) cnst_name, object_name(c.default_object_id) cnst_nonblank_name, m.text cnst_keys, 'D ' cnst_2type
		into #spcnsttabCashDEFAULT
		from	sys.columns c join syscomments m on m.id = c.rule_object_id
		
		create clustered index IX_#spcnsttabCashDEFAULT on #spcnsttabCashDEFAULT(object_id)
	end

	insert into #spcnsttab (cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type)
	select cnst_id,cnst_type,cnst_name,cnst_nonblank_name,cnst_keys, cnst_2type
	from #spcnsttabCashDEFAULT
	where object_id = @objid and objectproperty(cnst_id, 'IsConstraint') = 0

";

        #endregion

    }
}
