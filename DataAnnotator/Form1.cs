using DataAnnotator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DataAnnotator
{
    public partial class Form1 : Form
    {
        private string dataset_path;
        private string annotation_path;
        private List<Annotation> annotation_list = new List<Annotation>();
        private string[] environments = new string[3];
        private string[] agent_type = new string[3];
        private string[] agent_id_human = new string[3];
        private string[] agent_id_car = new string[3];
        private string[] agent_id_cyclist = new string[3];
        private string[] agent_pos_env = new string[3];
        private string[] agent_pos_to_robot = new string[3];
        private string[] agent_act = new string[3];
        private string[] robot_pos = new string[3];
        private string[] robot_act = new string[3];
        private string[] behaviors = new string[3];
        private string[] triggering_conditions = new string[3];
        private string[] ending_conditions = new string[3];
        private int listbox1_previous_index = -1;
        private bool annotation_changed = false;
        public Form1()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
            init_combobox_droplists();
            Text = "Data Annotator";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void init_combobox_droplists()
        {
            environments = new string[] {
                "Hallway (straight, curved, L-shaped)",
                "Room (small, medium, large)",
                "Doorway (narrow, wide)",
                "Open space (atrium, lobby)",
                "Elevator",
                "Sidewalk",
                "Crosswalk",
                "Staircase",
                "Ramp",
                "Parking lot" ,
                "Traffic signs",
                "Traffic lights",
                "Cones",
                "Construction site"
            };
            agent_type = new string[] {
                "Human",
                "Car",
                "Cyclist",
            };
            agent_id_human = new string[] {
                "[Color] man/woman in [Color] [Clothing Type]"
            };
            agent_id_car = new string[] {
                "[Color] [Type] [Make/Model (optional)]"
            };
            agent_id_cyclist = new string[] {
                "man/woman on [Color] bicycle, wearing [Color] [Clothing Type]"
            };
            agent_pos_env = new string[] {
                "man/woman on [Color] bicycle, wearing [Color] [Clothing Type]"
            };
            agent_pos_to_robot = new string[] {
                "at the robot's [1-12] o'clock"
            };
            agent_act = new string[] {
                "Approaching the robot from right/left/front/back/front left/front right/back left/back right side of the robot",
                "Moving in parallel with the robot on the left/right/front left/front right/back left/back right",
                "Stand/Sitting/Sitting down/Standing up on the right/left/front/back/front left/front right/back left/back right side of the robot",
                "Turning left/right/back from the right/left/front/back/front left/front right/back left/back right side of the robot",
                "Interacting with an object (e.g., drinking fountain, vending machine)",
                "Gesturing (hand wave, nodding, pointing)",
                "Talking with people one his right/left",
                "Stepping aside"
            };
            robot_pos = new string[] {
                "right/left/front/back/front left/front right/back left/back right",
                "from the opposite side of the doorway"
            };
            robot_act = new string[] { 
                "Moving forward",
                "Stopping",
                "Turning",
                "Changing speed (speeding up, slowing down)",
                "Waiting"
            };
            behaviors = new string[] { 
                "Robot yields to human",
                "Human yields to robot",
                "Both agents yield",
                "Robot waits for human to pass",
                "Human waits for robot to pass",
                "Robot navigates around human",
                "Human navigates around robot",
                "Robot asks human for assistance(e.g., to press a button)",
                "Human provides robot with instructions"
            };
            triggering_conditions = new string[] {
                "The person is in the way of the robot. The robot has to stop to avoid collision.",
                "The robot has to yield to persons on the crosswalk.",
                "The robot has to stop to avoid collision.",
                "The robot has to nudge to be socially compliant."
            };
            ending_conditions = new string[] {
                "Front is clear",
                "The person passes",
                "Out of the environment"
            };
        }

        private void refresh_combobox_droplists()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox8.Items.Clear();
            comboBox9.Items.Clear();
            comboBox10.Items.Clear();
            comboBox11.Items.Clear();
            comboBox1.Items.AddRange(environments);
            comboBox2.Items.AddRange(robot_pos);
            comboBox3.Items.AddRange(agent_act);
            comboBox4.Items.AddRange(behaviors);
            comboBox5.Items.AddRange(triggering_conditions);
            comboBox6.Items.AddRange(ending_conditions);
            comboBox7.Items.AddRange(robot_act);
            comboBox8.Items.AddRange(agent_pos_env);
            comboBox9.Items.AddRange(agent_pos_to_robot);
            // comboBox10.Items.AddRange();
            comboBox11.Items.AddRange(agent_type);   
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            comboBox7.SelectedIndex = -1;
            comboBox8.SelectedIndex = -1;
            comboBox9.SelectedIndex = -1;
            comboBox10.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            comboBox5.Text = null;
            comboBox6.Text = null;
            comboBox7.Text = null;
            comboBox8.Text = null;
            comboBox9.Text = null;
            comboBox10.Text = null;
            comboBox11.Text = null;
        }

        private void refresh_combobox_textbox()
        {
            refresh_combobox_droplists();
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            numericUpDown1.Text = null;
        }


        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }

        private void set_path(String path)
        {
            dataset_path = path;
            annotation_path = System.IO.Path.Combine(dataset_path, "annotations");
        }

        // private void 

        private void selectDatasetPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Please select a directory as a dataset path:";
            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = Environment.SpecialFolder.Desktop;
            DialogResult dialogResult = dialog.ShowDialog();
            System.Windows.Forms.DialogResult result = dialogResult;
            if (result == DialogResult.OK)
            {
                set_path(dialog.SelectedPath);
                refresh_listbox1();
                this.Text = "Selected Dataset Path: " + dialog.SelectedPath;
            }
        }
        public class BagVideoFile
        {
            public string file_path { get; set; }
            public string file_name { get; set; }
            public BagVideoFile(string file_path)
            {
                this.file_path = file_path;
                this.file_name = Path.GetFileName(file_path);
            }

            public override string ToString() { return this.file_name; }
        }

        private void refresh_listbox1() 
        {
            listBox1.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(dataset_path); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.avi"); //Getting Text files

            foreach (FileInfo file in Files)
            {
                listBox1.Items.Add(new BagVideoFile(file.Name));
            }
        }

        private string wrap_field(string s)
        {
            return "\"" + s + "\"";
        }

        private string unwrap_field(string s)
        {
            return s.Trim('"');
        }

        public class Annotation
        {
            public string environment { get; set; }
            public string robot_pos { get; set; }
            public string robot_act { get; set; }
            public string agent_type { get; set; }
            public string agent_num { get; set; }
            public string agent_id { get; set; }
            public string agent_pos_env { get; set; }
            public string agent_pos_to_robot { get; set; }
            public string agent_act { get; set; }
            public string behavior { get; set; }
            public string triggering_condition { get; set; }
            public string end_condition { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string description { get; set; }

            public Annotation()
            {
                environment = string.Empty;
                robot_pos = string.Empty;
                robot_act = string.Empty;
                agent_type = string.Empty;
                agent_num = string.Empty;
                agent_id = string.Empty;
                agent_pos_env = string.Empty;
                agent_pos_to_robot = string.Empty;
                agent_act = string.Empty;
                behavior = string.Empty;
                triggering_condition = string.Empty;
                end_condition = string.Empty;
                start = string.Empty;
                end = string.Empty;
                description = string.Empty;
            }
        }

        private void refresh_listbox2()
        {
            listBox2.Items.Clear();
            string annotation_path = Path.Combine(this.annotation_path, Path.GetFileNameWithoutExtension(listBox1.SelectedItem.ToString())+".csv");

            if (File.Exists(annotation_path)) 
            {
                read_annotation(annotation_path);
                foreach (Annotation anno in annotation_list)
                {
                    listBox2.Items.Add("Start: " + anno.start.ToString() + ", End: " + anno.end.ToString());
                }
            }
            refresh_combobox_textbox();
            annotation_changed = false;
        }


        public class LinqCSVReader
        {
            /// <summary>
            /// 使用File.ReadLines和LINQ读取CSV文件。
            /// </summary>
            /// <param name="filePath">CSV文件的路径。</param>
            public static IEnumerable<string[]> ReadCSV(string filePath)
            {
                // 使用LINQ查询将文件行转换为由逗号分隔的字符串数组的序列
                return File.ReadLines(filePath)
                           .Select(line => line.Split(','));
            }
        }

        public void write_csv(List<Annotation> annos, string write_path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(write_path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(write_path));
            }
            if (File.Exists(write_path))
            {
                File.Delete(write_path);
            }
            StreamWriter swl = new StreamWriter(write_path, true, Encoding.UTF8);
            foreach (Annotation anno in annos)
            {
                swl.Write(wrap_field(anno.environment) + "," + wrap_field(anno.robot_pos) + "," + wrap_field(anno.robot_act) + "," + wrap_field(anno.agent_num) + "," + wrap_field(anno.agent_type) + "," +
                    wrap_field(anno.agent_id) + "," + wrap_field(anno.agent_pos_env) + "," + wrap_field(anno.agent_pos_to_robot) + "," + wrap_field(anno.agent_act) + "," + wrap_field(anno.behavior) + "," + 
                    wrap_field(anno.triggering_condition) + "," + wrap_field(anno.end_condition) + "," + wrap_field(anno.start) + "," + wrap_field(anno.end) + "," + wrap_field(anno.description) + "\t\n");
            }
            swl.Close();
        }
        private void read_annotation(string annotation_path)
        {
            annotation_list.Clear();
            foreach (string[] fields in LinqCSVReader.ReadCSV(annotation_path))
            {
                Annotation anno = new Annotation();
                anno.environment = unwrap_field(fields[0]);
                anno.robot_pos = unwrap_field(fields[1]);
                anno.robot_act = unwrap_field(fields[2]);
                anno.agent_num = unwrap_field(fields[3]);
                anno.agent_type = unwrap_field(fields[4]);
                anno.agent_id = unwrap_field(fields[5]);
                anno.agent_pos_env = unwrap_field(fields[6]);
                anno.agent_pos_to_robot = unwrap_field(fields[7]);
                anno.agent_act = unwrap_field(fields[8]);
                anno.behavior = unwrap_field(fields[9]);
                anno.triggering_condition = unwrap_field(fields[10]);
                anno.end_condition = unwrap_field(fields[11]);
                anno.start = unwrap_field(fields[12]);
                anno.end = unwrap_field(fields[13]);
                anno.description = unwrap_field(fields[14]);
                annotation_list.Add(anno);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(Path.Combine(dataset_path, listBox1.Items[listBox1.SelectedIndex].ToString()));
            if (listbox1_previous_index == -1)
            {
                listbox1_previous_index = listBox1.SelectedIndex;
            }
            else 
            {
                if (listBox1.SelectedIndex != -1 && listbox1_previous_index != listBox1.SelectedIndex && annotation_changed)
                {
                    if ((int)MessageBox.Show("You haven't save your annotations for last video. Do you want to save it?", "Reminder", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == 1)
                    {
                        annotation_list[listBox2.SelectedIndex].environment = comboBox1.Text;
                        annotation_list[listBox2.SelectedIndex].robot_pos = comboBox2.Text;
                        annotation_list[listBox2.SelectedIndex].robot_act = comboBox7.Text;
                        annotation_list[listBox2.SelectedIndex].agent_num = numericUpDown1.Text;
                        annotation_list[listBox2.SelectedIndex].agent_type = comboBox11.Text;
                        annotation_list[listBox2.SelectedIndex].agent_id = comboBox10.Text;
                        annotation_list[listBox2.SelectedIndex].agent_pos_env = comboBox8.Text;
                        annotation_list[listBox2.SelectedIndex].agent_pos_to_robot = comboBox9.Text;
                        annotation_list[listBox2.SelectedIndex].agent_act = comboBox3.Text;
                        annotation_list[listBox2.SelectedIndex].behavior = comboBox4.Text;
                        annotation_list[listBox2.SelectedIndex].triggering_condition = comboBox5.Text;
                        annotation_list[listBox2.SelectedIndex].end_condition = comboBox6.Text;
                        annotation_list[listBox2.SelectedIndex].description = textBox3.Text;
                        annotation_list[listBox2.SelectedIndex].start = textBox1.Text;
                        annotation_list[listBox2.SelectedIndex].end = textBox2.Text;
                        listBox2.Items[listBox2.SelectedIndex] = "Start: " + annotation_list[listBox2.SelectedIndex].start + ", End: " + annotation_list[listBox2.SelectedIndex].end;
                        write_csv(annotation_list, Path.Combine(annotation_path, Path.GetFileNameWithoutExtension(listBox1.Items[listbox1_previous_index].ToString()) + ".csv"));
                        annotation_changed = false;
                    }
                }
                listbox1_previous_index = listBox1.SelectedIndex;
            }
            refresh_listbox2();
            axWindowsMediaPlayer1.URL = Path.Combine(dataset_path, listBox1.Items[listBox1.SelectedIndex].ToString());
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                //delete
                if (listBox1.SelectedIndex > -1)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }
            }
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                //delete
                if (listBox2.SelectedIndex > -1)
                {
                    listBox2.Items.Remove(listBox2.SelectedItem);
                }
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) {
                refresh_combobox_textbox();
                annotation_changed = false;
                return;
            }
            Annotation anno = annotation_list[listBox2.SelectedIndex];
            refresh_combobox_textbox();
            comboBox1.Text = anno.environment;
            if (comboBox1.Items.Contains(anno.environment))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(anno.environment);
            }
            comboBox2.Text = anno.robot_pos;
            if (comboBox2.Items.Contains(anno.robot_pos))
            {
                comboBox2.Text = anno.robot_pos;
                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(anno.robot_pos);
            }
            comboBox3.Text = anno.agent_act;
            if (comboBox3.Items.Contains(anno.agent_act))
            {
                comboBox3.SelectedIndex = comboBox3.Items.IndexOf(anno.agent_act);
            }
            comboBox4.Text = anno.behavior;
            if (comboBox4.Items.Contains(anno.behavior))
            {
                comboBox4.SelectedIndex = comboBox4.Items.IndexOf(anno.behavior);
            }
            comboBox5.Text = anno.triggering_condition;
            if (comboBox5.Items.Contains(anno.triggering_condition))
            {
                comboBox5.SelectedIndex = comboBox5.Items.IndexOf(anno.triggering_condition);
            }
            comboBox6.Text = anno.end_condition;
            if (!comboBox6.Items.Contains(anno.end_condition))
            {
                comboBox6.SelectedIndex = comboBox6.Items.IndexOf(anno.end_condition);
            }
            comboBox7.Text = anno.robot_act;
            if (!comboBox7.Items.Contains(anno.robot_act))
            {
                comboBox7.SelectedIndex = comboBox7.Items.IndexOf(anno.robot_act);
            }
            comboBox8.Text = anno.agent_pos_env;
            if (!comboBox8.Items.Contains(anno.agent_pos_env))
            {
                comboBox8.SelectedIndex = comboBox8.Items.IndexOf(anno.agent_pos_env);
            }
            comboBox9.Text = anno.agent_pos_to_robot;
            if (!comboBox9.Items.Contains(anno.agent_pos_to_robot))
            {
                comboBox9.SelectedIndex = comboBox9.Items.IndexOf(anno.agent_pos_to_robot);
            }
            comboBox11.Text = anno.agent_type;
            if (!comboBox11.Items.Contains(anno.agent_type))
            {
                comboBox11.SelectedIndex = comboBox11.Items.IndexOf(anno.agent_type);
            }
            comboBox10.Text = anno.agent_id;
            if (!comboBox10.Items.Contains(anno.agent_id))
            {
                comboBox10.SelectedIndex = comboBox10.Items.IndexOf(anno.agent_id);
            }
            textBox1.Text = anno.start.ToString();
            textBox2.Text = anno.end.ToString();
            textBox3.Text = anno.description.ToString();
            numericUpDown1.Text = anno.agent_num;
            annotation_changed = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                annotation_list.Add(new Annotation());
                annotation_list[annotation_list.Count - 1].environment = comboBox1.Text;
                annotation_list[annotation_list.Count - 1].robot_pos = comboBox2.Text;
                annotation_list[annotation_list.Count - 1].robot_act = comboBox7.Text;
                annotation_list[annotation_list.Count - 1].agent_num = numericUpDown1.Text;
                annotation_list[annotation_list.Count - 1].agent_type = comboBox11.Text;
                annotation_list[annotation_list.Count - 1].agent_id = comboBox10.Text;
                annotation_list[annotation_list.Count - 1].agent_pos_env = comboBox8.Text;
                annotation_list[annotation_list.Count - 1].agent_pos_to_robot = comboBox9.Text;
                annotation_list[annotation_list.Count - 1].agent_act = comboBox3.Text;
                annotation_list[annotation_list.Count - 1].behavior = comboBox4.Text;
                annotation_list[annotation_list.Count - 1].triggering_condition = comboBox5.Text;
                annotation_list[annotation_list.Count - 1].end_condition = comboBox6.Text;
                annotation_list[annotation_list.Count - 1].description = textBox3.Text;
                annotation_list[annotation_list.Count - 1].start = textBox1.Text;
                annotation_list[annotation_list.Count - 1].end = textBox2.Text;
                listBox2.Items.Add("Start: " + annotation_list[annotation_list.Count - 1].start + ", End: " + annotation_list[annotation_list.Count - 1].end);
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            }
            else 
            {
                annotation_list[listBox2.SelectedIndex].environment = comboBox1.Text;
                annotation_list[listBox2.SelectedIndex].robot_pos = comboBox2.Text;
                annotation_list[listBox2.SelectedIndex].robot_act = comboBox7.Text;
                annotation_list[listBox2.SelectedIndex].agent_num = numericUpDown1.Text;
                annotation_list[listBox2.SelectedIndex].agent_type = comboBox11.Text;
                annotation_list[listBox2.SelectedIndex].agent_id = comboBox10.Text;
                annotation_list[listBox2.SelectedIndex].agent_pos_env = comboBox8.Text;
                annotation_list[listBox2.SelectedIndex].agent_pos_to_robot = comboBox9.Text;
                annotation_list[listBox2.SelectedIndex].agent_act = comboBox3.Text;
                annotation_list[listBox2.SelectedIndex].behavior = comboBox4.Text;
                annotation_list[listBox2.SelectedIndex].triggering_condition = comboBox5.Text;
                annotation_list[listBox2.SelectedIndex].end_condition = comboBox6.Text;
                annotation_list[listBox2.SelectedIndex].description = textBox3.Text;
                annotation_list[listBox2.SelectedIndex].start = textBox1.Text;
                annotation_list[listBox2.SelectedIndex].end = textBox2.Text;
                listBox2.Items[listBox2.SelectedIndex] = "Start: " + annotation_list[listBox2.SelectedIndex].start + ", End: " + annotation_list[listBox2.SelectedIndex].end;

            }
            write_csv(annotation_list, Path.Combine(annotation_path, Path.GetFileNameWithoutExtension(listBox1.Items[listBox1.SelectedIndex].ToString()) + ".csv"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                refresh_combobox_textbox();
                return;
            }
            Annotation anno = annotation_list[listBox2.SelectedIndex];
            refresh_combobox_textbox();
            comboBox1.Text = anno.environment;
            if (comboBox1.Items.Contains(anno.environment))
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(anno.environment);
            }
            comboBox2.Text = anno.robot_pos;
            if (comboBox2.Items.Contains(anno.robot_pos))
            {
                comboBox2.Text = anno.robot_pos;
                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(anno.robot_pos);
            }
            comboBox3.Text = anno.agent_act;
            if (comboBox3.Items.Contains(anno.agent_act))
            {
                comboBox3.SelectedIndex = comboBox3.Items.IndexOf(anno.agent_act);
            }
            comboBox4.Text = anno.behavior;
            if (comboBox4.Items.Contains(anno.behavior))
            {
                comboBox4.SelectedIndex = comboBox4.Items.IndexOf(anno.behavior);
            }
            comboBox5.Text = anno.triggering_condition;
            if (comboBox5.Items.Contains(anno.triggering_condition))
            {
                comboBox5.SelectedIndex = comboBox5.Items.IndexOf(anno.triggering_condition);
            }
            comboBox6.Text = anno.end_condition;
            if (!comboBox6.Items.Contains(anno.end_condition))
            {
                comboBox6.SelectedIndex = comboBox6.Items.IndexOf(anno.end_condition);
            }
            comboBox7.Text = anno.robot_act;
            if (!comboBox7.Items.Contains(anno.robot_act))
            {
                comboBox7.SelectedIndex = comboBox7.Items.IndexOf(anno.robot_act);
            }
            comboBox8.Text = anno.agent_pos_env;
            if (!comboBox8.Items.Contains(anno.agent_pos_env))
            {
                comboBox8.SelectedIndex = comboBox8.Items.IndexOf(anno.agent_pos_env);
            }
            comboBox9.Text = anno.agent_pos_to_robot;
            if (!comboBox9.Items.Contains(anno.agent_pos_to_robot))
            {
                comboBox9.SelectedIndex = comboBox9.Items.IndexOf(anno.agent_pos_to_robot);
            }
            comboBox11.Text = anno.agent_type;
            if (!comboBox11.Items.Contains(anno.agent_type))
            {
                comboBox11.SelectedIndex = comboBox11.Items.IndexOf(anno.agent_type);
            }
            comboBox10.Text = anno.agent_id;
            if (!comboBox10.Items.Contains(anno.agent_id))
            {
                comboBox10.SelectedIndex = comboBox10.Items.IndexOf(anno.agent_id);
            }
            textBox1.Text = anno.start.ToString();
            textBox2.Text = anno.end.ToString();
            textBox3.Text = anno.description.ToString();
            numericUpDown1.Text = anno.agent_num;
            annotation_changed = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            annotation_list.Add(new Annotation());
            
            listBox2.Items.Add("Start: " + annotation_list[annotation_list.Count - 1].start.ToString() + ", End: " + annotation_list[annotation_list.Count - 1].end.ToString());
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select one annotation from the list!", "Warning!");
                return;
            }
            annotation_list.RemoveAt(listBox2.SelectedIndex);
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            listBox2.SelectedIndex = -1;
        }

        private void saveChangesToDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { return; }
            write_csv(annotation_list, Path.Combine(annotation_path, Path.GetFileNameWithoutExtension(listBox1.Items[listBox1.SelectedIndex].ToString()) + ".csv"));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || (axWindowsMediaPlayer1.playState != WMPLib.WMPPlayState.wmppsPaused && axWindowsMediaPlayer1.playState != WMPLib.WMPPlayState.wmppsPlaying))
            {
                MessageBox.Show("No video selected, or the video is in wrong state!", "Error");
                return;
            }
            string csv_file = Path.Combine(dataset_path, "timestamps", Path.GetFileNameWithoutExtension(listBox1.Items[listBox1.SelectedIndex].ToString()) + ".csv");
            if (File.Exists(csv_file))
            {
                string[] timestamps = File.ReadAllText(csv_file).ToString().Split(',');
                int idx = (int)Math.Floor(axWindowsMediaPlayer1.Ctlcontrols.currentPosition / 0.2);
                Clipboard.SetText(timestamps[idx]);
            }
            else
            {
                MessageBox.Show("Cannot find the timestamp information for this video!", "Error");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void comboBox6_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            annotation_changed = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex != -1)
            {
                if (comboBox11.SelectedIndex == 0) { comboBox10.Items.Clear(); comboBox10.Items.AddRange(agent_id_human); }
                if (comboBox11.SelectedIndex == 1) { comboBox10.Items.Clear(); comboBox10.Items.AddRange(agent_id_car); }
                if (comboBox11.SelectedIndex == 2) { comboBox10.Items.Clear(); comboBox10.Items.AddRange(agent_id_cyclist); }
            }
        }
    }
}
