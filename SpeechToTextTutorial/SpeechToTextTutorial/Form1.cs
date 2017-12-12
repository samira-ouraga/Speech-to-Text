using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace SpeechToTextTutorial
{
    public partial class Form1 : Form
    {
        public SpeechRecognitionEngine rec;
        public Grammar grammar;
        public Thread RecThread;
        public Boolean RecognizerState = true;

             
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GrammarBuilder build = new GrammarBuilder();
            build.AppendDictation();
            grammar = new Grammar(build);

            rec = new SpeechRecognitionEngine();
            rec.LoadGrammar(grammar);
            rec.SetInputToDefaultAudioDevice();
            rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(rec_SpeechRecognized);

            RecognizerState = true;
            RecThread = new Thread(new ThreadStart(RecThreadFunction));
            RecThread.Start();

        }

       

        private void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!RecognizerState)
            {
                return;
            }
            this.Invoke((MethodInvoker)delegate
            {
                textBox1.Text += (" " + e.Result.Text.ToLower());
            });  

        }

        public void RecThreadFunction()
        {
            while (true)
            {
                try
                {
                    rec.Recognize();
                }
                catch
                {

                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            RecognizerState = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RecognizerState = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RecThread.Abort();
            RecThread = null;

            rec.UnloadAllGrammars();
            rec.Dispose();

            grammar = null;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.bing.com/search?q=" + textBox1.Text);

        }
    }
}
