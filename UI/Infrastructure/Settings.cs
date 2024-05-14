using System;
using System.IO;
using System.Xml.Serialization;

namespace UI.Infrastructure
{
    public class Settings
    {
        private static Settings _current;
        private static readonly object _syncObj = new object();
        private const string FileName = "Settings.xml";

        public static Settings Current
        {
            get
            {
                if (_current != null)
                    return _current;

                lock (_syncObj)
                {
                    if (_current == null)
                    {
                        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
                        if (!File.Exists(fileName))
                            return _current = GetDefault();

                        var serializer = new XmlSerializer(typeof(Settings));
                        using (var stream = File.Open(fileName, FileMode.Open))
                            _current = (Settings)serializer.Deserialize(stream);
                    }
                }

                return _current;
            }
        }

        private static Settings GetDefault()
        {
            return new Settings
            {
                Locale = "en-US",
            };
        }

        public static void SaveSettings()
        {
            if (_current == null)
                return;

            lock (_syncObj)
            {
                var serializer = new XmlSerializer(typeof(Settings));
                using (var stream = File.Create(AppDomain.CurrentDomain.BaseDirectory + FileName))
                    serializer.Serialize(stream, _current);
            }
        }

        #region properties
        public string Locale { get; set; }


        #endregion

    }
}
