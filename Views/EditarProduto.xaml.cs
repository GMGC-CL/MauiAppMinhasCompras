using MauiAppMinhasCompras.Models;
using System.Diagnostics;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{

    public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Categoria = txt_categoria.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Comprado = chk_comprado.IsChecked,
                DataCompra = chk_comprado.IsChecked ? dtp_dataCompra.Date : (DateTime?)null
            };

            int result = await App.Db.Update(p);
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