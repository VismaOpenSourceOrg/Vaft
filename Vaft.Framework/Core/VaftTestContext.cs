using System.Linq;

namespace Vaft.Framework.Core
{
    public class VaftTestContext
    {
        private string _className;

        public string MethodName { get; set; }

        public string ClassName
        {
            get { return _className; }
            set
            {
                string[] tokens = value.Split('.');
                _className = tokens.Last();
            }
        }

        public string FullName
        {
            get { return ClassName + "." + MethodName; }
        }
    }
}
