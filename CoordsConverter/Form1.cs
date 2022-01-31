using Coordinates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CoordsConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    var lines = File.ReadAllLines(filePath);
                    for (var i = 0; i < lines.Length; i += 1)
                    {
                        var line = lines[i];
                        CoordsList1.Items.Add(line);
                    }
                }
            }

            FilePath.Text = filePath;
        }

        private void CoordsList1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.C))
            {
                Clipboard.SetText(this.CoordsList1.SelectedItem.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var item in CoordsList1.Items)
            {
                var x2 = item.ToString();
                double x = double.Parse(item.ToString().Split(',')[0]);
                double y = double.Parse(item.ToString().Split(',')[1]);

                Converters.itm2wgs84((float)y, (float)x, out double lat, out double lon);

                var Mylat = lat;
                var Mylon = lon;
                var new_Coords = Mylat.ToString() + ',' + Mylon.ToString();
                CoordsList2.Items.Add(new_Coords);


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog oSaveFileDialog = new SaveFileDialog();
            oSaveFileDialog.Filter = "All files (*.*) | *.*";
            if (oSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = oSaveFileDialog.FileName;
                string extesion = Path.GetExtension(fileName);
                StreamWriter sw = new StreamWriter(oSaveFileDialog.OpenFile());
                if(sw!=null) {
                foreach (var it in CoordsList2.Items)
                {
                        sw.WriteLine(it);
                }                    
                    sw.Close();
                }
            }

            MessageBox.Show(string.Format("File Saved in {0}", oSaveFileDialog.FileName));
        }
    }
}
