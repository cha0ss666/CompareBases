using CompareBases.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareBases
{
    public class SQLScriptDB_PgSql : ISqlScript
    {
        public bool WithTable { get; set; }
        public bool TableWithTrigger { get; set; }
        public string FilterPrefix { get; set; }
        public List<string> FilterIgnoreByPrefix { get; set; }
        public List<string> FilterIgnoreByPostfix { get; set; }

        public string GetScriptCountObjects()
        {
            return GetScriptPrepareWhere() + @"
--процедуры/функции
select cast(count(1) as int)
from pg_proc p
left join pg_namespace n on n.oid = p.pronamespace
cross join lateral (select '[' || n.nspname || '].[' || p.proname || ']' as name, n.nspname as schemaname) obj
" + GetScriptWhere();
        }

        private string GetScriptSchemas(bool useFilterPrefix = true)
        {
            return @" 
union all
SELECT -1*ROW_NUMBER() over (order by schema_name) as object_id
	, ' ' as type
    , schema_name as ""schemaName""
    , ' ' as name
    , 'CREATE SCHEMA [' || schema_name || ']' as definition
    , now() as create_date
    , now() as modify_date
FROM information_schema.schemata
    "
                + (!useFilterPrefix || string.IsNullOrWhiteSpace(FilterPrefix) ? "" : " where '[' || schema_name || ']' like '"
                    + GetStringLike(FilterPrefix)
                    + "%'");
        }

        private string GetScriptWhere(string types = null, bool useFilterPrefix = true)
        {
            return @"
    left join filterIgnore fi on obj.name like fi.mask
    where obj.schemaname not in ('pg_catalog','information_schema')
        and fi.mask is null
"
                + (!useFilterPrefix || string.IsNullOrWhiteSpace(FilterPrefix) ? "" : " and obj.name like '"
                    + GetStringLike(FilterPrefix)
                    + "%'")
                ;
        }

        private string GetStringLike(string orig)
        {
            return orig.Replace("'", "''").Replace("_", "[_]");
        }

        private string GetScriptPrepareWhere()
        {
            return @"
DROP TABLE IF EXISTS filterIgnore;
CREATE TEMPORARY TABLE filterIgnore(mask text);
"
                + FilterIgnoreByPrefix.Where(f => !string.IsNullOrWhiteSpace(f)).Aggregate(""
                    , (s, f) => (s == "" ? "insert into filterIgnore(mask) " : s + Environment.NewLine + "union ")
                        + "select '" + GetStringLike(f) + "%'")
                + Environment.NewLine
                + FilterIgnoreByPostfix.Where(f => !string.IsNullOrWhiteSpace(f)).Aggregate(""
                    , (s, f) => (s == "" ? "insert into filterIgnore(mask) " : s + Environment.NewLine + "union ")
                        + "select '%" + GetStringLike(f) + "'")
                + Environment.NewLine;
        }

        public string GetScriptDefinitionObjects()
        {
            return GetScriptPrepareWhere() + @"
--процедуры/функции
select p.oid as object_id
	, case when p.prokind = 'f' then 'FN' else 'P' end as type
	, n.nspname as ""schemaName""
    , p.proname as name
    , pg_get_functiondef(p.oid) as definition
    , now() as create_date
    , now() as modify_date
from pg_proc p
left join pg_namespace n on n.oid = p.pronamespace
cross join lateral (select '[' || n.nspname || '].[' || p.proname || ']' as name, n.nspname as schemaname) obj
" + GetScriptWhere() + GetScriptSchemas();

        }
    }
}
