using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace FlowerTitan.TemplateGenerator
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplateGenerator
    {
        private static TemplateGenerator templateGenerator = null;

        private static TemplateGeneratorWindow templateGeneratorWindow = null;

        private TemplateGenerator() {}

        public static TemplateGenerator GetInstance(TemplateGeneratorWindow tGW)
        {
            if (templateGenerator == null)
            {
                templateGenerator = new TemplateGenerator();
            }
            templateGeneratorWindow = tGW;
            return templateGenerator;
        }

        public void GenerateTemplate()
        {
            
        }
    }
}
