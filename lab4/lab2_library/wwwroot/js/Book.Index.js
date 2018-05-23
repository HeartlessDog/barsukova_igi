askServerAndRenderList('Books/SortedList');

$('#sort-name').click(function () {
    askServerAndRenderList('Books/SortedList');
    saveToSession('Books/SaveFiltration');
});

$('#find').keyup(function () {
    filterList();
    saveToSession('Books/SaveFiltration');
});

filterList();