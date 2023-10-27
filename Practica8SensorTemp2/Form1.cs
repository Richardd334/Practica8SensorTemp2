using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Practica8SensorTemp2
{
    public partial class Form1 : Form
    {
        delegate void SetTextDelegate(string value);
        public SerialPort ArduinoPort { 
            get; 
        }


        public Form1()
        {
            InitializeComponent();
            ArduinoPort= new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM3";
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 500;
            ArduinoPort.WriteTimeout = 500;
            ArduinoPort.DataReceived +=  new SerialDataReceivedEventHandler(DataReceivedHandler);

            this.btnConectar.Click += btnConectar_Click;
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
            EscribirTxt(dato);
        }

        private void EscribirTxt(string dato)
        {
            if(InvokeRequired)
                try
                {
                    Invoke(new SetTextDelegate(EscribirTxt), dato);
                }
                catch
                {
                    lblTemp.Text = dato;
                }
            else
            {
                lblTemp.Text = dato;
            }
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
            btnDesconectar.Enabled = true;
            try
            {
                if (!ArduinoPort.IsOpen)
                    ArduinoPort.Open();
                if(int.TryParse(txtLimite.Text, out int temperatureLimit))
                {
                    //Convierte el valor a una cadena y luego en un arreglo en bytes
                    string limitString = temperatureLimit.ToString();
                    ArduinoPort.Write(limitString);

                }
                else
                {
                    MessageBox.Show("Ingresa un valor númerico válido en el TextBox del limite de la temperatura");
                }
                lblConexion.Text = "Conexión OK";
                lblConexion.ForeColor = System.Drawing.Color.Lime;
            }
            catch
            {
                MessageBox.Show("Configure el puerto de comunicaion correcto o desconecte");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            if (ArduinoPort.IsOpen)
                ArduinoPort.Close();
            lblConexion.Text = "Desconectado";
            lblConexion.ForeColor = System.Drawing.Color.Red;
            lblTemp.Text = "00.0";
        }
    }
}
