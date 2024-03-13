namespace Livros.API.Model{
    public class Livro{
        public int Id { get; set; }
        public int Paginas { get; set; }
        public String? Titulo { get; set; }
        public int Ano { get; set; }
        public String? Autor { get; set; }
    }
}