document.getElementById('openModalButton').addEventListener('click', function ()
{
    $.ajax({
        url: 'путь_к_вашему_файлу.cshtml',
        type: 'GET',
        success: function (data) {
            // Вставляем полученное содержимое в модальное окно
            $('#myModal .modal-body').html(data);
            // Открываем модальное окно
            $('#myModal').modal('show');
        }
    }); 
});