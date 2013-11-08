using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Player
{
    public static class console
    {
        private const string _path_ = "./log.txt";

        static console()
        {

        }

        public static string logenum(System.Collections.IEnumerable e)
        {
            string output = "[";
            foreach(var i in e)
            {
                output += i.ToString() + ",";
            }
            output += "]";

            return output;
        }

        public static void log(params object[] what)
        {
            using (StreamWriter st = new StreamWriter(_path_, true))
            {
                string output = "";
                foreach ( var i in what )
                {
                    output += i.ToString();
                }
                st.Write(output + '\n');
            }
        }

        public static void Clear()
        {
            File.Delete(_path_);
        }
    }
}
