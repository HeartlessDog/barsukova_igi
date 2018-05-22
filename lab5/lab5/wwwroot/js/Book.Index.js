
askServerAndRenderList('Books/SortedList', 0);

$('#sort-name, #sort-price').click(function () {
    askServerAndRenderList('Books/SortedList', $('#currentPage').val());
    saveToSession('Books/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Books/SaveFiltration');
});

filterList()