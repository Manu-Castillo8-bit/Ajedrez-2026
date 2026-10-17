using System;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Ajedrez;
public static class Z3 {
    static FieldInfo F(Type t, string n){ return t.GetField(n, BindingFlags.NonPublic|BindingFlags.Instance); }
    public static void Run(bool dark) {
        Tema.SetTheme(dark);
        Sesion.NombreUsuario = "Manuel";
        Type t = typeof(Ajedrez.Perfil);
        Form f = (Form)Activator.CreateInstance(t);
        f.Show();
        var lbl = (Label)F(t,"descripcion").GetValue(f);
        string s = "Me gusta jugar ajedrez y damas con mis amigos los viernes";
        if (s.Length > 50) s = s.Substring(0, 50);
        lbl.Text = s;
        int needed = TextRenderer.MeasureText(lbl.Text, lbl.Font, new Size(lbl.Width, int.MaxValue), TextFormatFlags.WordBreak).Height;
        var btn = (Button)F(t,"editar_descripcion_btn").GetValue(f);
        Console.WriteLine((dark?"OSCURO":"CLARO ") + " len=" + s.Length + " font=" + lbl.Font.Size + "pt box=" + lbl.Width + "x" + lbl.Height + " neededH=" + needed + " fits=" + (needed <= lbl.Height) + " btnVisible=" + btn.Visible);
        f.Dispose();
    }
    public static void Main(string[] args) { Run(false); Run(true); }
}
