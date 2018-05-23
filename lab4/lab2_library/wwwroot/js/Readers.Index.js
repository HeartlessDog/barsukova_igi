askServerAndRenderList('Readers/SortedList');

$('#sort-name').click(function () {
    askServerAndRenderList('Readers/SortedList');
    saveToSession('Readers/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Readers/SaveFiltration');
});

filterList();