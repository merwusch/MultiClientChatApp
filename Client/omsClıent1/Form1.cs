using Matriks.Oms.Core.Logging;
using Matriks.Oms.Core.Network;
using Matriks.Oms.Core.Network.Client;
using Matriks.Oms.Core.Network.Socket;
using Matriks.Oms.Core.Threading;
using Matriks.Oms.Core.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffer = Matriks.Oms.Core.Network.Buffer;

namespace omsClıent1
{
    public partial class Form1 : Form
    {
        IClientAdapter client;
        ILogAdapter logger;

        IJobQueue<string> jobQueue;

        public Form1()
        {
            InitializeComponent();
            Load += FromLoad;
        }

        private void FromLoad(object sender, EventArgs e)
        {
            logger = new DefaultLogAdapter();
            var packageCollector = new LinePacketAggregator();
            packageCollector.SetBufferManager(new Buffer());
            client = new SocketClientAdapter(packageCollector, logger);
            client.Connected += Connected;
            client.Received += Received;

            JobBuilder.Create("Timer").OnAction(OnTick).Schedule(TimeSpan.FromSeconds(1)).Build().Start();

            jobQueue = new JobQueue<string>("q", OnMessage);
            jobQueue.Start();
        }

        private void OnMessage(string obj)
        {
            Console.WriteLine(obj);
        }

        private void OnTick(IJobDataContext jobDataContext)
        {
            Console.WriteLine(DateTime.Now);
        }

        private void Received(object sender, ClientReceiveEventArgs e)
        {

            int listede_yok = 0;//yok
            string gelen = Encoding.Default.GetString(e.Packet).ToString();//serverdan gelen mesaj
            if (gelen.Contains("sil*"))
            {
                string parcala = gelen.Substring(4, (gelen.Length - 4));
                Console.WriteLine("degerim  " + parcala);
                Invoke(new Action(() => {
                    for (int j = 0; j < listBox1.Items.Count; j++)//list boxtanda kaldır
                    {
                        if (listBox1.Items[j].Equals(parcala))
                        {
                            listBox1.Items.RemoveAt(j);

                        }
                    }
                }));
             }
            else if (gelen.Contains("@"))//içerisinde @ içeriyorsa clienti listeye eklicez
            {

                Invoke(new Action(() =>
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)//listedeki itemler kadar dön
                    {
                        if (listBox1.Items[i].ToString().Equals(gelen))//listede varsa o client
                        {
                            listede_yok = 1;//var
                        }
                    }
                    if (listede_yok == 0)//yoksa  ekle clienti
                    {
                        string ben = "@" + txName.Text;
                        if (ben.Equals(gelen))//kendimi ekleme
                        {

                        }
                        else
                        {
                            listBox1.Items.Add(gelen);
                        }
                    }
                }));

            }
            else
            {
                //label3.Text = (gelen);
                Invoke(new Action(() =>
                {
                    rb_chat.AppendText(gelen + "\n");
                }));
            }
        }

        private void Connected(object sender, ClientConnectedArgs e)
        {
            Invoke(new Action(() => {
                byte[] buffer = Encoding.ASCII.GetBytes("@@" + txName.Text);//ismimizin başına 2 tane @@ koydum belli olsun
                client.Send(buffer);//veriyi gönderdim servera
                label3.Text = ("servere bağlandı!");//servere bağlandı
            }));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            client.Connect("127.0.0.1", 4000);
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string tmpStr = "";
            foreach (var item in listBox1.SelectedItems)//listboxtaki seçili itemlere
            {

                tmpStr = listBox1.GetItemText(item);//isimlerini
                byte[] buffer = Encoding.ASCII.GetBytes(tmpStr + " :" + txt_text.Text + "*" + txName.Text);//byte çevir
                client.Send(buffer);//ve gönder ip ve porta
                Thread.Sleep(20);//yapmasanda olur(fakat 4 cliente bırden mesaj gonderınce dıgerlerine gıtmeyebılir)

            }
            if (tmpStr.Equals(""))
            {
                MessageBox.Show("lütfen listeden değer seçiniz");
            }
            else
            {
                rb_chat.AppendText(txName.Text + ": " + txt_text.Text + "\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            jobQueue.Push("DENEMEME + " + DateTime.Now.ToString());
        }
    }
}
