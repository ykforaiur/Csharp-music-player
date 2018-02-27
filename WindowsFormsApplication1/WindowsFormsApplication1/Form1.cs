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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //关闭自动播放
            musicplayer.settings.autoStart = false;
        }
        /// 播放或者暂停
        bool b = true;
        private void btnplayorpause_Click_1(object sender, EventArgs e)
        {
            
            if (btnplayorpause.Text == "播放")
            {
                if (b)
                {
                    //获得选中的歌曲  让音乐从头播放
                    musicplayer.URL = listpath[listBox1.SelectedIndex];
                }
                musicplayer.Ctlcontrols.play();
                btnplayorpause.Text = "暂停";
            }
            else if (btnplayorpause.Text == "暂停")
            {
                musicplayer.Ctlcontrols.pause();
                btnplayorpause.Text = "播放";
                b = false;
            }
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            musicplayer.Ctlcontrols.stop();
            btnplayorpause.Text = "播放";
            b = true;
        }

        /// 打开对话框 选择音乐
        List<string> listpath = new List<string>();
        private void btnopen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"";
            ofd.Filter = "mp3文件|*.mp3|音乐文件|*.wav|所有文件|*.*";
            ofd.Title = "请选择音乐文件";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            string[] path = ofd.FileNames;
            for (int i = 0; i < path.Length; i++)
            {
                listpath.Add(path[i]);
                listBox1.Items.Add(Path.GetFileName(path[i]));
            }
        }


        /// 双击播放对应的音乐
        private void listBox1_DoubleClick_1(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请首先选择音乐文件");
                return;
            }
            try
            {
                musicplayer.URL = listpath[listBox1.SelectedIndex];
                musicplayer.Ctlcontrols.play();
                btnplayorpause.Text = "暂停";
                IsExistLrc(listpath[listBox1.SelectedIndex]);
            }
            catch {  }
        }
        /// 点击上一曲
        private void btnlast_Click(object sender, EventArgs e)
        {
         
            int index = listBox1.SelectedIndex;
            listBox1.SelectedIndices.Clear();
            index--;
            
            if (index < 0)
            {
                index = listBox1.Items.Count - 1;
            }
            listBox1.SelectedIndex = index;
            musicplayer.URL = listpath[index];
            try
            {
                musicplayer.Ctlcontrols.play();
                btnplayorpause.Text = "暂停";                
                IsExistLrc(listpath[listBox1.SelectedIndex]);
            }
            catch{  }
        }
        /// 点击下一曲   
        private void btnnext_Click(object sender, EventArgs e)
        {
            
            //获得当前选中项的索引
            int index = listBox1.SelectedIndex;

            //清空所有选中项的索引
            if (radioButton1.Checked == true || radioButton2.Checked == true)
            {
                listBox1.SelectedIndices.Clear();
                index++;
                if (index == listBox1.Items.Count)
                {
                    index = 0;
                }
            }
            
            else if (radioButton3.Checked == true)
            {
                Random rd = new Random();
                int i = rd.Next(0, listBox1.Items.Count);
                listBox1.SelectedIndices.Clear();
                index = i;
            }
            //将改变后的索引重新的赋值给当前选中项的索引
            listBox1.SelectedIndex = index;
            musicplayer.URL = listpath[index];
            try
            {
                musicplayer.Ctlcontrols.play();
                btnplayorpause.Text = "暂停";
                IsExistLrc(listpath[listBox1.SelectedIndex]);
            }
            catch{  }
        }



        /// 点击删除 选中项
        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //首先获得要删除的歌曲的数量
            int count = listBox1.SelectedItems.Count;
            for (int i = 0; i < count; i++)
            {
                //先删集合
                listpath.RemoveAt(listBox1.SelectedIndex);
                //再删列表
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

            }

        }
        /// 放大音量
        private void button2_Click(object sender, EventArgs e)
        {
            musicplayer.settings.volume += 5;
        }
        /// 减小声音
        private void button1_Click(object sender, EventArgs e)
        {
            musicplayer.settings.volume -= 5;
        }
        /// 时间显示
        private void timer1_Tick(object sender, EventArgs e)
        {
            //如果播放器的状态等于正在播放中

            if (musicplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                lblInformation.Text = musicplayer.currentMedia.duration.ToString() + "\r\n" + musicplayer.currentMedia.durationString + "\r\n" + musicplayer.Ctlcontrols.currentPosition.ToString() + "\r\n" + musicplayer.Ctlcontrols.currentPositionString;
            }
        }
        private void musicplayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //  MessageBox.Show(musicplayer.playState.ToString());
            if (musicplayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                //获得当前选中项的索引
                int index = listBox1.SelectedIndex;

                //清空所有选中项的索引
                if (radioButton1.Checked == true)
                {
                    listBox1.SelectedIndices.Clear();
                    index++;
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                }
                else if(radioButton2.Checked == true)
                {
                    //单曲循环不改变索引值
                }
                else if(radioButton3.Checked == true)
                {
                    Random rd = new Random();
                    int i = rd.Next(0, listBox1.Items.Count);
                    listBox1.SelectedIndices.Clear();
                    index = i;
                }
                //将改变后的索引重新的赋值给当前选中项的索引
                listBox1.SelectedIndex = index;
                musicplayer.URL = listpath[index];

            }
            if (musicplayer.playState == WMPLib.WMPPlayState.wmppsReady)
            {
                try
                {
                    musicplayer.Ctlcontrols.play();
                }
                catch { }
            }

        }
        //开始做歌词
        void IsExistLrc(string songPath)
        {
            //清空两个集合的内容
            listTime.Clear();
            listLrcText.Clear();
            songPath += ".lrc";
            if (File.Exists(songPath))
            {
                //读取歌词文件
                string[] lrcText = File.ReadAllLines(songPath, Encoding.Default);
                //格式化歌词
                FormatLrc(lrcText);
            }
            else//不存在歌词
            {
                label2.Text = "---------歌词未找到---------";
            }

        }
        //存储时间
        List<double> listTime = new List<double>();
        //存储歌词
        List<string> listLrcText = new List<string>();


        /// 格式化歌词
        void FormatLrc(string[] lrcText)
        {
            for (int i = 0; i < lrcText.Length; i++)
            {
                //截取左右时间点以及歌词
                string[] lrcTemp = lrcText[i].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lrcNewTemp = lrcTemp[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                double time = double.Parse(lrcNewTemp[0]) * 60 + double.Parse(lrcNewTemp[1]);
                //把截取出来的时间加到泛型集合中
                listTime.Add(time);
                //把这个时间所对应的歌词存储到泛型集合中
                listLrcText.Add(lrcTemp[1]);
            }
        }

        
        /// 播放歌词
        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listTime.Count-1; i++)
            {
                if (musicplayer.Ctlcontrols.currentPosition >= listTime[i] && musicplayer.Ctlcontrols.currentPosition < listTime[i + 1])
                {
                    label2.Text = listLrcText[i];
                }
            }

        }

    }
}
