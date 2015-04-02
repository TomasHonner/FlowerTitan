using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowerTitan.TemplateGenerator
{
    public partial class TemplateGeneratorWindow : Form
    {
        TemplateGenerator templateGenerator;

        public TemplateGeneratorWindow()
        {
            InitializeComponent();
        }

        private void TemplateGeneratorWindow_Load(object sender, EventArgs e)
        {
            templateGenerator = TemplateGenerator.GetInstance(this);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            templateGenerator.GenerateTemplate(10);
        }
    }
}
