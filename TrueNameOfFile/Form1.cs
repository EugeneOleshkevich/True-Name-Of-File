using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using TagLib;
using System.Reflection;
namespace TrueNameOfFile
{
    public partial class Form1 : Form
    {
        class trueName
        {
            public string[] files;
            public void trueOfName()
            {
                int ind = files[0].LastIndexOf('\\');                                                           //Search index last slash
                string pathDirectory = files[0].Substring(0, (ind + 1));                                        //a path to the folder with the files
                int cutFirstIndex; 
                string nameFile, nameFileOld;                                                                               
                var mp3File = TagLib.File.Create(files[0]);
                string[] checkFiles;                                                                            //check mp3-files
                bool checkingFiles;                                                                             //check that there are no files with the same name.
                bool result;                                                                                    //eng check
                char[] letters;                                                                                 //string to char-check
                int checkVersion = 0;

                for (int i = 0; i < files.Length; i++)                                                          //cycle file
                {
                   
                                                     
                    cutFirstIndex = files[i].LastIndexOf(".mp3");                                               //check mp3
                    if (cutFirstIndex != -1)                                         
                    {
                        mp3File = TagLib.File.Create(files[i]);                                                 //create taglib-file
                        checkingFiles = true;
                        if (mp3File.Tag.Title != null)                                                          //check, title not empty
                        {
                            if (files[i] != null)
                            {
                                ind = files[i].LastIndexOf('\\');
                                nameFileOld = files[i].Substring(ind + 1, (files[i].Length - (ind + 1)));       //check name file
                                nameFile = mp3File.Tag.Title.ToString();                                        //check title
                                checkVersion = 0;
                                //---check, that tag-name is English
                                letters = nameFile.ToCharArray();   
                                result = true;
                                for (int k = 0; k < letters.Length; k++)                                        
                                {
                                    int charValue = System.Convert.ToInt32(letters[k]);
                                    if (charValue > 128)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                                //---
                                if (nameFileOld.CompareTo(nameFile) != 0 && result == true)                                       //compare them to do unnecessary operations
                                {
                                    checkFiles = Directory.GetFiles(pathDirectory, "*mp3");                     //check that there are no files with the same name.
                                    for (int j = 0; j < checkFiles.Length; j++)
                                    {
                                        if ((pathDirectory + nameFile + ".mp3").CompareTo(checkFiles[j]) == 0)  
                                        {
                                            checkingFiles = false;
                                            break;
                                        }
                                    }
                                    checkVersion = 0;
                                    //---found the unique name
                                    if (checkingFiles == false)                                                 
                                    {
                                        int j=0;
                                        checkVersion = 1;
                                        checkFiles = Directory.GetFiles(pathDirectory, "*mp3");
                                        do
                                        {

                                            if ((pathDirectory + nameFile + "(" + checkVersion + ")" + ".mp3").CompareTo(checkFiles[j]) == 0)
                                            {
                                                checkVersion++;
                                                j = 0;
                                            }
                                            j++;
                                        } while (j < checkFiles.Length);
                                        checkingFiles = true;
                                    }
                                    //---
                                    if (checkingFiles != false)
                                    {
                                        if (mp3File.Tag.FirstPerformer != null)                                 //if file have the Artist
                                        {
                                            if (checkVersion != 0)
                                            {
                                                System.IO.File.Move(files[i], pathDirectory + mp3File.Tag.FirstPerformer.ToString() + " - " + nameFile + "(" + checkVersion + ")" + ".mp3");
                                            }
                                            else
                                            {
                                                System.IO.File.Move(files[i], pathDirectory + mp3File.Tag.FirstPerformer.ToString() + " - " + nameFile + ".mp3");
                                            }
                                        }
                                        else
                                        {
                                            if (checkVersion != 0)
                                            {
                                                System.IO.File.Move(files[i], pathDirectory + nameFile + "(" + checkVersion + ")" + ".mp3");
                                            }
                                            else
                                            {
                                                System.IO.File.Move(files[i], pathDirectory + nameFile + ".mp3");
                                            }
                                        }
                                    }
                                    checkVersion = 0;
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Completed!");
            }
            public trueName()
            {

            }
        }
        trueName fixedName = new trueName();

        public Form1()
        {
            InitializeComponent();
            Assembly ass = null;
            try
            {
                ass = Assembly.Load("taglib-sharp, Version=2.1.0.0, Culture=neutral, PublicKeyToken=db62eba44689b5b0");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Not found \"taglib-sharp (Version=2.1.0.0, Culture=neutral, PublicKeyToken=db62eba44689b5b0)\"!\nCheck taglib-sharp.dll file.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {

                fixedName.files = Directory.GetFiles(fbd.SelectedPath);                                     //Get all files in folder
                /*
                 * contents of the array files:
                 * files[0] = C:\ff\text.txt
                 * files[1] = C:\ff\text(2).txt
                 */
                int ind = fixedName.files[0].LastIndexOf('\\');                                             //Search index last slash
                string pathDirectory = fixedName.files[0].Substring(0, (ind + 1));                         //a path to the folder with the files
                textBox1.Text = pathDirectory;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
         // textBox2.Text = mp3File.Tag.Title;
            fixedName.trueOfName();
        }
    }
}
