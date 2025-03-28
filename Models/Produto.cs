using SQLite;
using System.ComponentModel;

namespace MauiAppMinhasCompras.Models
{
    public class Produto : INotifyPropertyChanged
    {
        string _descricao;
        string _categoria;
        bool _comprado;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool Comprado
        {
            get => _comprado;
            set
            {
                if (_comprado != value)
                {
                    _comprado = value;
                    OnPropertyChanged(nameof(Comprado));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Descricao { 
            get => _descricao; 
            set
            {
                if(value == null) 
                {
                    throw new Exception("Por favor, preencha a descrição");
                }

                _descricao = value;
            }
        }
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a categoria");
                }

                _categoria = value;
            }
        }
        public double Quantidade {  get; set; }
        public double Preco {  get; set; }
        public double Total { get => Quantidade * Preco; }
        public DateTime? DataCompra { get; set; }
    }
}
