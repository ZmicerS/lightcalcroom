$(document).ready(function () {
    // alert("document).ready");
    //   alert("дададада");
    GetAllColumns();
    $("#editKofOtrz").click(function (event) {
        event.preventDefault();
        EditClmn();
    });

    //      alert("after edit clmn");

    $("#addKofOtrz").click(function (event) {
        event.preventDefault();
        AddClmn();
    });


   
});
function say(data) { console.log(data); }
//
// Получение всехколонок по ajax-запросу
function GetAllColumns() {
    $("#createBlock").css('display', 'inline-block');
    $("#editBlock").css('display', 'none');
    //
    $("#editId").val("");
    $("#editPtlk").val("");
    $("#editSteny").val("");
    $("#editPol").val("");
    //
    $("#addId").val("");
    $("#addPtlk").val("");
    $("#addSteny").val("");
    $("#addPol").val("");
    //
    val = $("#TblKfId").val();
    //  alert(val);
    $.ajax({
        url: '/IsprStrcMtrKfIsp/PlcDanKfOtrz',
        type: "POST",
        dataType: "JSON",
        data: { TblKfId: val },
        success: WriteResponse
    });
};
//

function WriteResponse(data) {
  //  alert(data);
    var strResult = "<table><th>Номер колонки</th><th>Потолок</th><th>Стены</th><th>Пол</th>";
    $.each(data, function (index, data) {
        // alert(data.Id);
        strResult += "<tr><td>" + data.NmrCl + "</td><td> " + data.Ptlk + "</td><td>" +
   data.Steny + "</td><td>" + data.Pol +
   "</td><td><a id='editItem' data-item='" + data.Id + "' onclick='EditItem(this);' >Редактировать</a></td>"
        $("#NmrCln").val(data.NmrCl);
    })
    strResult += "</table>";
    $("#tableBlock").html(strResult);
}

// добавление колонки
function AddClmn() {
  //  alert("Добавить колонку ");
    var elm = $('#addPtlk');
    say(elm);
    var elmval = elm.val();
    var ptrn = $('#addPtlk').attr("pattern");
    var reg = new RegExp(ptrn)
    var us = reg.test(elmval);
    if (!us) {
        elm.focus();
        return;
    }
    //
    var elms = $('#addSteny');
    say(elms);
    var elmvals = elms.val();
    var ptrns = $('#addSteny').attr("pattern");
    var reg = new RegExp(ptrns)
     us = reg.test(elmvals);
    if (!us) {
        elms.focus();
        return;
    }
    //
    var elmp = $('#addPol');
    say(elmp);
    var elmvalp = elmp.val();
    var ptrnp = $('#addPol').attr("pattern");
    var reg = new RegExp(ptrnp)
    us = reg.test(elmvalp);
    if (!us) {
        elmp.focus();
        return;
    }
    //
    var clmnk = {
        // Id: $('#editId').val(),
        TblKfId: $('#TblKfId').val(),
        Ptlk: $('#addPtlk').val(),
        Steny: $('#addSteny').val(),
        Pol: $('#addPol').val(),
        NmrCl: $("#NmrCln").val()
    };
    $.ajax({
        url: '/IsprStrcMtrKfIsp/DbvlColKofOtr/',
        type: 'POST',
        dataType: "JSON",
        data: clmnk,
        success: function (data) {
            //    alert("УСПЕХКОЛОНКА");
            //GetAllBooks();
            GetAllColumns();
        },
        complete: function () {
            //    alert("Завершение выполнения");
        },
        error: function (x, y, z) {
            //alert("/TabKofIsp/EditColKofOtr/ -- неудача");
            alert(x + '\n' + y + '\n' + z);
        }
    })

};

// обработчик редактирования
function EditItem(el) {
 //   alert("Редактировать");
    // получаем id редактируемого объекта
    var id = $(el).attr('data-item');
    GetColKofOtr(id);
};

// запрос книги на редактирование
function GetColKofOtr(id) {
    $.ajax({
        url: '/IsprStrcMtrKfIsp/GetColKofOtr/',
        type: 'GET',
        dataType: 'json',
        data: { Id: id },
        success: function (data) {
          //  alert("ShowBook(data)");
            //  alert(data);
            ShowCol(data);
        },
        error: function (x, y, z) {
          //  alert("/TabKofIsp/GetColKofOtr/ -- неудача")
            alert(x + '\n' + y + '\n' + z);
        }
    })
};

// вывод данных редактируемой книги в поля для редактирования
function ShowCol(clnm) {
    //alert(clnm);
    if (clnm != null) {
        $("#createBlock").css('display', 'none');
        $("#editBlock").css('display', 'inline-block');
        $("#editId").val(clnm.Id);
        $("#editTblKfId").val(clnm.TblKfId);
        $("#editPtlk").val(clnm.Ptlk);
        $("#editSteny").val(clnm.Steny);
        $("#editPol").val(clnm.Pol);
    }
    else {
        alert("Такая колонка не существует");
    }
};


// редактирование колонки
function EditClmn() {
    var elm = $('#editPtlk');
    say(elm);
    var elmval = elm.val();
    var ptrn = $('#editPtlk').attr("pattern");
    var reg = new RegExp(ptrn)
    var us = reg.test(elmval);
    if (!us) {
        elm.focus();
        return;
    }
    
    var elms = $('#editSteny');
    say(elms);
    var elmvals = elms.val();
    var ptrns = $('#editSteny').attr("pattern");
    var reg = new RegExp(ptrns)
    us = reg.test(elmvals);
    if (!us) {
        elms.focus();
        return;
    }
    //
    var elmp = $('#editPol');
    say(elmp);
    var elmvalp = elmp.val();
    var ptrnp = $('#editPol').attr("pattern");
    var reg = new RegExp(ptrnp)
    us = reg.test(elmvalp);
    if (!us) {
        elmp.focus();
        return;
    }
    //
    var clmnk = {
        Id: $('#editId').val(),
        TblKfId: $('#editTblKfId').val(),
        Ptlk: $('#editPtlk').val(),
        Steny: $('#editSteny').val(),
        Pol: $('#editPol').val()
    };
    //
    $.ajax({
        url: '/IsprStrcMtrKfIsp/EditColKofOtr/',
        type: 'POST',
        dataType: "JSON",
        data: clmnk,
        success: function (data) {
            //    alert("УСПЕХКОЛОНКА");
            //GetAllBooks();
            GetAllColumns();
        },
        complete: function () {
            //    alert("Завершение выполнения");
        },
        error: function (x, y, z) {
            //alert("/TabKofIsp/EditColKofOtr/ -- неудача");
            alert(x + '\n' + y + '\n' + z);
        }
    });

}

