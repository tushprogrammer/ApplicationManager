namespace ApplicationManager.Entitys
{
    //значения на странице "главная" - названия блоков и значения описания элементов с гл. страницы
    public class MainPage
    {
        private int id;
        public int Id { get { return id; }  set { id = value; } }

        //техническое имя (названия блоков и поля на главной странице)
        private string name;
        public string Name { get { return name; } set { name = value; } }

        ////ссылка на блоки
        //текущее значение (какое админ написал)
        private string value;
        public string Value { get { return value; } set { this.value = value; } }
    }
}
