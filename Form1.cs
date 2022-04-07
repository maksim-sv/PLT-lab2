namespace PLT_lab2
{
    public partial class Form1 : Form
    {
        enum state { H, C1, C, S1, K, S, Err }
        state cs;
        state CS
        {
            get { return cs; }
            set { cs = value; CurStLabel.Text = value.ToString(); }
        }
        string onTape = "";
        string OnTape
        {
            get { return onTape; }
            set { onTape = value; Tape.Text = value; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(filename);
            fileText = System.Text.RegularExpressions.Regex.Replace(fileText, @"\s", "");
            textBox1.Text = fileText;
        }


        private void CheckBtn_Click(object sender, EventArgs e)
        {

            do
            {
                if (OnTape == "")
                {
                    if (run == false)
                    {
                        CS = state.H;
                        OnTape = textBox1.Text;
                        run = true;
                        return;
                    }
                    else
                    {
                        OnEmptyTape();
                    }
                    return;
                }
            }
            while (MakeStep());
        }

        bool run = false;

        void OnEmptyTape()//цепочка закончилась
        {
            if (CS == state.S)
                MessageBox.Show( "Цепочка принята","", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show($"Автомат не в конечном состоянии\nПоследнее состояние: {CS}\n", "Цепочка не принята", MessageBoxButtons.OK, MessageBoxIcon.Error);
            run = false;
            CurStLabel.Text = "";
        }
        private void StepBtn_Click(object sender, EventArgs e)
        {
            if (OnTape == "")
            {
                if (run == false)
                {
                    CS = state.H;
                    OnTape = textBox1.Text;
                    run = true;
                }
                else
                {
                    OnEmptyTape();
                }
                return;
            }
            MakeStep();
        }
        bool MakeStep()
        {
            char c = OnTape[0];
            OnTape = OnTape.Remove(0, 1);
            switch (CS)
            {
                case state.H:
                    if (c == '(')
                    {
                        CS = state.C1;
                        return true;
                    }
                    else if (c == '{')
                    {
                        CS = state.K;
                        return true;
                    }
                    else goto default;

                case state.K:
                    if (c == 'a' || c == '(' || c == '*' || c == ')' || c == '{')
                    {
                        return true;
                    }
                    else if (c == '}')
                    {
                        CS = state.S;
                        return true;
                    }
                    else goto default;

                case state.C1:
                    if (c == '*')
                    {
                        CS = state.C;
                        return true;
                    }
                    else goto default;

                case state.C:
                    
                    if (c == '*' && OnTape.LastIndexOf('*')==-1)
                    {
                        CS = state.S1;
                        return true;
                    }
                    else if(c == 'a' || c == '{' || c == '}' || c == '(' || c == '*' || c == ')')
                    {
                        return true;
                    }
                    else goto default;

                case state.S1:
                    if (c == ')')
                    {
                        CS = state.S;
                        return true;
                    }
                    else goto default;

                default:
                    if (c != 'a' && c != '(' && c != '*' && c != ')' && c != '{' && c != '}') { MessageBox.Show($"символ \"{c}\" не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else { MessageBox.Show($"q( {CS} , {c}... )├─q.Error", "ошибочное состояние", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    run = false;
                    OnTape = "";
                    CurStLabel.Text = "";
                    return false;
            }
        }
    }
}
