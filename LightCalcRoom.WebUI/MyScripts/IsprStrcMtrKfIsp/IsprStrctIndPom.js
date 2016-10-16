$(document).ready(function () {
    
    GetAllIndPom();
    $("#editIndPom").click(function (event) {
        event.preventDefault();
        EditIndxPom();
    });
    $("#addIndPom").click(function (event) {
        event.preventDefault();
        AddIndxPom();
           
    });
});
function say(data) {
    //console.log(data);
}
//
// Получение всехколонок по ajax-запросу
function GetAllIndPom() {
    //   alert("GetAllIndPom");
    $("#createBlock").css('display', 'inline-block');
    $("#editBlock").css('display', 'none');
    //
    $("#editId").val("");
    $("#editTblKfId").val("");
    $("#editIndPm").val("");
    //
    $("#addId").val("");
    $("#addIndPm").val("");
    //
    val = $("#TblKfId").val();
    $.ajax({
        url: '/IsprStrcMtrKfIsp/PlcDanIndxPm',
        type: "POST",
        dataType: "JSON",
        data: { TblKfId: val },
        success: WriteResponse
    });
};


function WriteResponse(data) {
   
    var strResult = "<table border=0><th>Номер колонки</th><th>Индекс помещения</th>";
    $.each(data, function (index, data) {
        
        strResult += "<tr><td>" + data.NmrRw + "</td><td> " + data.IndxPm + "</td>" +
             "<td><a id='editItem' data-item='" + data.Id + "' onclick='EditItem(this);' >Редактировать</a></td>"
        $("#NmrRow").val(data.NmrRw);
    })
    strResult += "</table>";
    $("#tableBlock").html(strResult);
}

function EditItem2(e)
{
  
}

function AddIndxPom() {
   
    var elm = $('#addIndPm');
    say(elm);
    var elmval = elm.val();
    var ptrn = $('#addIndPm').attr("pattern");
    
    var reg = new RegExp(ptrn)
    var us = reg.test(elmval);
    if (!us) {
        elm.focus();
        return;
    }
    var ipm = {
        Id: 0,
        TblKfId: $('#TblKfId').val(),
        IndPm: elmval,
        NmrRw: $('#NmrRow').val()
    }
    $.ajax({
        url: '/IsprStrcMtrKfIsp/DbvlIndxPm/',
        type: 'POST',
        dataType: "JSON",
        data: ipm,
        success: function (data) {
            
            GetAllIndPom();
        },
        complete: function () {
            
        },
        error: function (x, y, z) {
            
            alert(x + '\n' + y + '\n' + z);
            alert(x.responseText);
        }
    });
}

    // обработчик редактирования
    function EditItem(el) {
     //       
        // получаем id редактируемого объекта
        $("#createBlock").css('display', 'none');
        $("#editBlock").css('display', 'inline-block');
        var id = $(el).attr('data-item');
        GetIndPom(id);
    };

    //
    //запрос на редактирование
    function GetIndPom(id) {
        $.ajax({
            url: '/IsprStrcMtrKfIsp/GetIndxPm/',
            type: 'POST',
            dataType: 'json',
            data: { Id: id },
            success: function (data) {
               
                ShowIndPom(data);
            },
            error: function (x, y, z) {
            
                alert(x + '\n' + y + '\n' + z);
            }
        })
    };
    //
    function ShowIndPom(ipm) {
        if (ipm != null) {
            $("#editId").val(ipm.Id);
            $("#editTblKfId").val(ipm.TblKfId);
            $("#editIndPm").val(ipm.IndxPm);
        }
        else {
            alert("Такая колонка не существует");
        }
    };
    //

    function EditIndxPom() {
        var elm = $('#editIndPm');
        
        var elmval = elm.val();
        var ptrn = elm.attr("pattern");
     
        var reg = new RegExp(ptrn)
        var us = reg.test(elmval);
        if (!us) {
            elm.focus();
            return;
        }
        var ipm = {
            Id: $('#editId').val(),
            TblKfId: $('#editTblKfId').val(),
            IndPm: $('#editIndPm').val()
        };
        $.ajax({
            url: '/IsprStrcMtrKfIsp/EditIndxPm/',
            type: 'POST',
            dataType: "JSON",
            data: ipm,
            success: function (data) {
                
                GetAllIndPom();
            },
            complete: function () {
                
            },
            error: function (x, y, z) {
                
                alert(x + '\n' + y + '\n' + z);
                alert(x.responseText);
            }
        });

    }
