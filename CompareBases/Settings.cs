using CompareBases.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CompareBases
{
    public static class Settings
    {
        public const string ProgramParametersFileName = "csvn.xml";
        private static string TimeFormatString = "yyyy'-'MM'-'dd' 'HH'.'mm'.'ss'.'fffffff";

        public static ProgramParameters Param
        {
            get
            {
                if (m_Param == null) ReloadProgramParameters();
                return m_Param;
            }
        }

        private static ProgramParameters m_Param = null;

        public static void ReloadProgramParameters()
        {
            var serializer = new XmlSerializer(typeof(ProgramParameters));
            using (var fp = File.OpenRead(ProgramParametersFileName))
            {
                m_Param = (ProgramParameters)serializer.Deserialize(fp);
            }
            if (string.IsNullOrEmpty(m_Param.SVNCommandLog))
                m_Param.SVNCommandLog = @"TortoiseProc.exe /command:log /path:""{0}""";
        }

        public static void SaveProgramParameters()
        {
            var paramFileOldName = Path.Combine(Path.GetDirectoryName(ProgramParametersFileName)
                , Path.GetFileNameWithoutExtension(ProgramParametersFileName)
                + " " + DateTime.Now.ToString(TimeFormatString, CultureInfo.InvariantCulture)
                + Path.GetExtension(ProgramParametersFileName));
            if (File.Exists(ProgramParametersFileName))
                File.Move(ProgramParametersFileName, paramFileOldName);

            var serializer = new XmlSerializer(typeof(ProgramParameters));
            using (var fp = File.OpenWrite(ProgramParametersFileName))
            {
                serializer.Serialize(fp, m_Param);
            }
        }

        public static bool ExistFileParam()
        {
            return File.Exists(ProgramParametersFileName);
        }

        public static string CreateDefaultFileParam()
        {
            m_Param = new ProgramParameters();
            m_Param.SVNPath = @"Путь к папки со схемой проекта, например: С:\Projects\AIS_SN\AIS.SN.Database\1.Schema";
            m_Param.SnapshotPath = @"Путь куда будут сохраняться снапшоты";
            m_Param.SVNCommandLog = @"TortoiseProc.exe /command:log /path:""{0}""";
            
            SaveProgramParameters();

            return ProgramParametersFileName;
        }
    }
}
