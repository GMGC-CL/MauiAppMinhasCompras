using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public List<string> ListaCategorias { get; set; }
    public Produto Produto { get; set; }

    public EditarProduto(Produto produto)
    {
        InitializeComponent();

        Produto = produto;
        ListaCategorias = new List<string> { "Alimentos", "Bebidas", "Limpeza", "Higiene", "Outros" };
        BindingContext = this;
        txt_categoria.SelectedItem = Produto.Categoria;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto.Descricao = txt_descricao.Text;
            Produto.Categoria = txt_categoria.SelectedItem?.ToString();
            Produto.Quantidade = Convert.ToDouble(txt_quantidade.Text);
            Produto.Preco = Convert.ToDouble(txt_preco.Text);
            Produto.Comprado = chk_comprado.IsChecked;
            Produto.DataCompra = chk_comprado.IsChecked ? dtp_dataCompra.Date : (DateTime?)null;

            int result = await App.Db.Update(Produto);
            if (result > 0)
            {
                await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao atualizar o produto", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
        {
            dtp_dataCompra.Date = DateTime.Today;
        }
    }
}