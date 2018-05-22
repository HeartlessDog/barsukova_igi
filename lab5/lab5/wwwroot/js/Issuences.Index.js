askServerAndRenderList('Issuences/SortedList', 0);

$('#sort-name, #sort-price').click(function () {
    askServerAndRenderList('Issuences/SortedList', $('#currentPage').val());
    saveToSession('Issuences/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Issuences/SaveFiltration');
});

filterList();

