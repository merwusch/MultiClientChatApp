using Matriks.Oms.Core.Logging;
using Matriks.Oms.Core.Network;
using Matriks.Oms.Core.Network.Server;
using Matriks.Oms.Core.Network.Socket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffer = Matriks.Oms.Core.Network.Buffer;

namespace omsServer1
{
    public partial class Form1 : Form
    {
        IServerAdapter server;
        ILogAdapter logger;        
        public Form1()
        {
            InitializeComponent();
            Load += FormLoad;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            logger = new DefaultLogAdapter();
            var packageCollector = new LinePackageAggregator();
            packageCollector.SetBufferManager(new Buffer());
            server = new SocketServerAdapter(packageCollector, logger);
            server.Connected += Connected;
            server.Received += Received;

        }

        private void Received(object sender, ServerReceiveArgs e)
        {
            
        }

        private void Connected(object sender, ServerConnectedArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }
    }
}
