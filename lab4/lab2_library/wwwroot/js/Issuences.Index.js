askServerAndRenderList('Issuences/SortedList');

$('#sort-name').click(function () {
    askServerAndRenderList('Issuences/SortedList');
    saveToSession('Issuences/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Issuences/SaveFiltration');
});

filterList();



