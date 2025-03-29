using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public RelatorioCategoria()
    {
        InitializeComponent();
    }

    private async void FiltrarProdutos(object sender, EventArgs e)
    {
        try
        {
            DateTime inicio = dtp_inicio.Date;
            DateTime fim = dtp_fim.Date;

            var produtos = await App.Db.GetByPeriodo(inicio, fim);

            var produtosAgrupados = produtos
                .GroupBy(p => p.Categoria)
                .Select(g => new ProdutoCategoria
                {
                    Categoria = g.Key,
                    PrecoTotal = (decimal)g.Sum(p => p.Preco * p.Quantidade),
                })
                .ToList();

            lst_produtos_categoria.ItemsSource = produtosAgrupados;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var produtoCategoria = e.SelectedItem as ProdutoCategoria;
            DisplayAlert("Categoria Selecionada", produtoCategoria?.Categoria ?? "Nenhuma categoria", "OK");
        }
    }

    private async void CalcularTotal(object sender, EventArgs e)
    {
        try
        {
            if (lst_produtos_categoria.ItemsSource is List<ProdutoCategoria> produtosAgrupados)
            {
                decimal soma = produtosAgrupados.Sum(p => p.PrecoTotal);

                string msg = $"O total é {soma:C}";
                await DisplayAlert("Total dos Produtos", msg, "OK");
            }
            else
            {
                await DisplayAlert("Aviso", "Nenhum produto encontrado!", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    public class ProdutoCategoria
    {
        public string Categoria { get; set; }
        public decimal PrecoTotal { get; set; }
    }
}