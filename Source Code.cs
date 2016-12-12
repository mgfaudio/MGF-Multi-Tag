using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{

    public partial class Form1 : Form
    {
        public int midipitch(string myfilename)
        {
            int oct = 0;
            int offset = 0;
            int rootkey = 0;
            for (int indexy = 1; indexy < 4; indexy = indexy + 1)
            {
                int thispos = myfilename.Length - indexy - 4;
                char blobbochar = myfilename[thispos];
                string blobbo = "" + blobbochar;
                int j;
                bool getnumber = Int32.TryParse(blobbo, out j);
                if (getnumber)
                {
                    oct = 12 * Int32.Parse(blobbo) + 24;
                }
                switch (blobbochar)
                {
                    case '#':
                        offset = 1;
                        break;
                    case 'C':
                        rootkey = 0;
                        break;
                    case 'D':
                        rootkey = 2;
                        break;
                    case 'E':
                        rootkey = 4;
                        break;
                    case 'F':
                        rootkey = 5;
                        break;
                    case 'G':
                        rootkey = 7;
                        break;
                    case 'A':
                        rootkey = 9;
                        break;
                    case 'B':
                        rootkey = 11;
                        break;
                }

            }
            return oct + offset + rootkey;
        }

        public void writebyte(string myfilename, int midipitch)
        {

            Stream outStream = File.Open(myfilename, FileMode.Open);
            int pitchposition = Convert.ToInt32(outStream.Length - 24);
            label2.Text = "" + pitchposition;
            outStream.Seek(pitchposition, SeekOrigin.Begin);
            outStream.WriteByte(Convert.ToByte(midipitch));
            outStream.Seek(0, SeekOrigin.Begin);
            //string newfolder = "C:\Max Temp";
            byte[] bytesInStream = new byte[outStream.Length];
            outStream.Read(bytesInStream, 0, bytesInStream.Length);
            outStream.Close();
            File.WriteAllBytes(myfilename, bytesInStream);
        }

        public void checkforchunk(string myfilename)
        { 
            string genericchunk = "smpl$...........°(..9.......................";
            Stream outStream = File.Open(myfilename, FileMode.Open);
            outStream.Seek((outStream.Length - 44), SeekOrigin.Begin);
            byte[] checkbyte = new byte[1];
            outStream.Read(checkbyte, 0, 1);
            bool spresent = false;
            bool mpresent = false;
            if (checkbyte[0] == Convert.ToByte(115))
            {
                spresent = true;
            }
            outStream.Seek((outStream.Length - 43), SeekOrigin.Begin);
            checkbyte = new byte[1];
            outStream.Read(checkbyte, 0, 1);
            if (checkbyte[0] == Convert.ToByte(109))
            {
                mpresent = true;
            }

            if (!spresent & !mpresent)
            {
                label2.Text = "Sample chunk not present. Adding...";
                outStream.Seek((outStream.Length), SeekOrigin.Begin);
                for (int i = 0; i < 44; i++)
                {
                    byte chunkbyte = Convert.ToByte(genericchunk[i]);
                    if (chunkbyte == Convert.ToByte('.'))
                        {
                        chunkbyte = 0;
                        }
                    outStream.WriteByte(chunkbyte);
                }
                outStream.Seek(0, SeekOrigin.Begin);
                byte[] bytesInStream = new byte[outStream.Length];
                outStream.Read(bytesInStream, 0, bytesInStream.Length);
                outStream.Close();
                File.WriteAllBytes(myfilename, bytesInStream);
                }

        }
        public Form1()
        {
            InitializeComponent();
            
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    string stringo = file;
                    int thismidipitch = midipitch(stringo);
                    label1.Text = "" + thismidipitch;
                    checkforchunk(stringo);
                    writebyte(stringo, thismidipitch);
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
