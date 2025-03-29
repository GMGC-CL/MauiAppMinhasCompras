using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public List<string> ListaCategorias { get; set; }
    public NovoProduto()
    {
        InitializeComponent();
        ListaCategorias = new List<string> { "Alimento", "Bebida", "Doce", "Ferramenta", "Higiene", "Limpeza", "Utensilio" };
        BindingContext = this; 
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Categoria = txt_categoria.SelectedItem?.ToString(),
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}