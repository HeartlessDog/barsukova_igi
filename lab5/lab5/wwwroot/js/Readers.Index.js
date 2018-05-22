askServerAndRenderList('Readers/SortedList', 0);

$('#sort-name, #sort-price').click(function () {
    askServerAndRenderList('Readers/SortedList', $('#currentPage').val());
    saveToSession('Readers/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Readers/SaveFiltration');
});

filterList();

