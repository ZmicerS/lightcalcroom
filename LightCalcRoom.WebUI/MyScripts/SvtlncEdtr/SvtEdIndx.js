$(document).ready(function () {
   
    $('#TblKfId').on('change', fncchangetblname);
    $('#SvtlId').on('change', fchngsvtl)
    $('input:radio[name="dobizmsvt"]').on('change', fncdbizm)
});

function fncdbizm()
{
   
    val = $(this).val();
    $('#SvtlId').val("-1");
    $('#TblKfId').val("-1");
    $('#tbl').empty();
    $("#IdLmp").val("-1");
    $("#izmSvtlnk").val("");
    $("#izmLampa").val("");
    $("#izmKol").val("");
    $("#izmPwr").val("");
    $("#izmPotok").val("");
    zprvklsubmit('sbmtizmsvtl', 'disabled');
    $("#dbvlSvtlnk").val("");
    $("#dbvlLampa").val("");
    $("#dbvlKol").val("");
    $("#dbvlPwr").val("");
    $("#dbvlPotok").val("");
    zprvklsubmit('sbmsvtl', 'disabled');
    $("#dblSvtl").toggle();
    $("#izmSvtl").toggle();
}

function fncchangetblname() {
    
    var val = this.value;
  
    zprvklsubmit('sbmsvtl', 'disabled');
    $.ajax({
        url: '/SvtlncEdtr/PlcTblKfAjx',
        type: 'POST',
        dataType: 'Json',
        data: { TblId: val },
        success: scsajxtblkf
    })
};

function say(data) {
   // console.log(data);
};

function scsajxtblkf(data) {
   
    $('#tbl').empty();
  
    if (typeof data !== 'undefined')
        {
        if (data!=null && data.Id>0)
        {
            rabtabl(data);
            zprvklsubmit('sbmsvtl', 'enabled');
        }       
     }
};

function rabtabl(data) {

    var Id = data.TblId;
    var Nazva = data.Nazva;
    var klstlb = data.Kolcln;
    var klstrk = data.Kolstr;
    var arshpk = data.MsKfOtrz;
    var arlv = data.MsIndPm;
    var arznc = data.MsKf;
    var table = $('<table border="1"></table>')
    var capt = $('<caption>' + Nazva + '</caption>')
    table.append(capt);
  
    //шапка
    for (i = 0; i < 3; i++) {

        switch (i) {
            case 0:
                var row = $('<tr><td>потолок</td></tr>')
                break
            case 1:
                var row = $('<tr><td>стены</td></tr>')
                break
            case 2:
                var row = $('<tr><td>пол</td></tr>')
                break
            default:
        }    
    for (j = klstlb * i; j < (klstlb * (i + 1)) ; j++) {
   
        row.append('<td>' + arshpk[j] + '</td>');
    }
 
    table.append(row);
    }
    //
    //данные таблицы
    for (i = 0; i < klstrk; i++) {
        var row = $('<tr><td>' + arlv[i] + '</td></tr>')
        for (j = klstlb * i; j < (klstlb * (i + 1)) ; j++) {
        
            row.append('<td>' + arznc[j] + '</td>');
        }
        table.append(row);
    }
    //
    $('#tbl').empty();
    $('#tbl').append(table);

}


function zprvklsubmit(nzvid, status) {// блокирование и разблокирование кнопки submit
    var sbmt = document.getElementById(nzvid);
    
    if (sbmt.type=="submit")
    {
        //alert(sbmt);
        if (status == 'disabled')
        {
            sbmt.setAttribute('disabled', 'true')
        }
      else
      {
        var atr=sbmt.getAttribute('disabled');
        if (atr) 
            sbmt.removeAttribute('disabled');
        }
    }//if (sbmt.type=="submit")
}


function fchngsvtl() {
    $("#IdLmp").val("-1");
    $("#izmSvtlnk").val("");
    $("#izmLampa").val("");
    $("#izmKol").val("");
    $("#izmPwr").val("");
    $("#izmPotok").val("");
    zprvklsubmit('sbmtizmsvtl', 'disabled');
    val = this.value;
  
    $.ajax({
        url: '/SvtlncEdtr/PlcSvtlAjx',
        type: 'POST',
        dataType: 'Json',
        data: { SvtlId: val },
        success: scssvtl
    });
};

function scssvtl(data)
{
   
    if (data.Lmpest > 0)
    {
        $("#IdLmp").val(data.IdLmp);
        $("#izmSvtlnk").val(data.NzvSvtl);
        $("#izmLampa").val(data.NazvaLmp);
        $("#izmKol").val(data.KlLmp);
        $("#izmPwr").val(data.Wt);
        $("#izmPotok").val(data.Lumen);
        zprvklsubmit('sbmtizmsvtl', 'enabled');
    }
}