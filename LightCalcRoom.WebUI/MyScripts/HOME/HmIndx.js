$(document).ready(function () {
    // alert("document).ready");
    //    alert("дададада");
    $('#slsvt').on('change', fncchangesvtl);
    $('form#frmrscht').submit(fncrscht);
    fncopnstrnc();
  //  var val = $('#slsvt').val();
  //  alert(val);
});
function say(data) {
    //console.log(data);
}
//
function fncopnstrnc()
{
    var val = $('#slsvt').val();
    var svt = $('#slsvt')[ 0 ]; ;
   // alert(val);
    fncchangesvtl.call(svt);
    return;
    udlkofotrz();//удаление данных в выборе отражения
    udltblkfisp()//удаляем таблицу коф. использования
    udltlmpstrnc();//удаляем информацию о лампе в светильнике
    // alert(val);
    zprvklsubmit('sbmtrscht', 'disabled');
    $.ajax({
        url: '/Home/SmnSvtlnc',
        type: "POST",
        dataType: "JSON",
        data: { svtlid: val },
        success: scsajaxsmnsvtl,
        complete: function () {
            // alert("!!Завершение выполнения");
            say('Завершение выполнения');
        }
    });
}
//
function fncchangesvtl() {
  // alert(this.value);
  var val = this.value;
  // alert(this.value);
    udlkofotrz();//удаление данных в выборе отражения
    udltblkfisp()//удаляем таблицу коф. использования
    udltlmpstrnc();//удаляем информацию о лампе в светильнике
    udlrscht();
    // alert(val);
    zprvklsubmit('sbmtrscht', 'disabled');
    $.ajax({
        url: '/Home/SmnSvtlnc',
        type: "POST",
        dataType: "JSON",
        data: { svtlid: val },
        success: scsajaxsmnsvtl,
        complete: function () {
           // alert("!!Завершение выполнения");
               say('Завершение выполнения');
        }
    });
}


function scsajaxsmnsvtl(data) {
   // alert("@@scsajax");
  //  alert(data);
    // say('scsajaxsmnsvtl');
    if (data.TblKfId > 0 && data.Kolstr > 0 && data.Kolcln > 0)
    {
        //  alert(data);
        vstvktbl(data);//вставляем таблицу коф. использования      
        vstvkslct("otrz", data.Kolcln, data.Shapka);
        var lest = data.Lmpest;
        if (lest == 1) {
            var nzvlmp = data.NazvaLmp;
            var lmn = data.Lumen;
            var kllmp = data.KlLmp;
            var wt = data.Wt;
            vstvtxtid("nzvlmp", nzvlmp);
            vstvtxtid("svtptk", lmn);
            vstvtxtid("pwr", wt);
            vstvtxtid("kl", kllmp);
            // vstvktbl();
            zprvklsubmit('sbmtrscht', 'enabled');
        }

    }
}

function vstvktbl(data) {//вставляем таблицу коф. использования
    var klstrk = data.Kolstr;
    var klstlb = data.Kolcln;
    var arshpk = data.Shapka;
    var arlv = data.Levo;
    var arznc = data.Znac;
  //  var elm = document.getElementById("kut");
    //  var vt = elm.getElementsByTagName("table");
    var elm = document.getElementById("tuttbl");
    var fragment = document.createDocumentFragment();
    var table = document.createElement("table");
    table.setAttribute("border", "1");
    var capt = document.createElement('caption');
    capt.appendChild(document.createTextNode("Таблица коф. исп-"+data.NzvTbl));
    table.appendChild(capt);
    say(table)
    var thead = document.createElement('thead');
    for (i = 0; i < 3; i++) {
        var tr = document.createElement('tr');
        var td = document.createElement('th')
        switch (i) {
            case 0:
                td.appendChild(document.createTextNode('потолок'))
                break;
            case 1:
                td.appendChild(document.createTextNode('стены'))
                break
            case 2:
                td.appendChild(document.createTextNode('пол'))
                break
            default:
        }
        tr.appendChild(td)
        for (j = klstlb * i; j < (klstlb * (i + 1)) ; j++) {
            td = document.createElement('th')
            td.appendChild(document.createTextNode(arshpk[j]))
            tr.appendChild(td)
        }
        say(tr);
        //   table.appendChild(tr)
        thead.appendChild(tr)
        say(thead);
    }//for shapka
    table.appendChild(thead)
    say(table);
    //
    //данные таблицы
    var tbody = document.createElement('tbody');
    table.appendChild(tbody);
    say(table);
    //
    say(table);
    //данные таблицы
    var tbody = document.createElement('tbody');
    for (i = 0; i < klstrk; i++) {
        tr = document.createElement('tr');
        td = document.createElement('td');
        //var row = $('<tr><td>' + arlv[i] + '</td></tr>')
        td.appendChild(document.createTextNode(arlv[i]));
        tr.appendChild(td);

        for (j = klstlb * i; j < (klstlb * (i + 1)) ; j++) {
            //  row.append('<td>' + arznc[j] + '</td>');
            td = document.createElement('td');
            //var row = $('<tr><td>' + arlv[i] + '</td></tr>')
            td.appendChild(document.createTextNode(arznc[j]));
            tr.appendChild(td);

        }
        tbody.appendChild(tr)
        say(tbody);
    }
    table.appendChild(tbody);
    say(table);
    //
    fragment.appendChild(table);
   // alert(fragment)
    elm.appendChild(fragment)
    say(fragment);
    say(elm);
}


function udltblkfisp() {//удаляем таблицу коф. использования
    //  document.getElementById("tblkfisp");
  //  ZnsZncDvInpt("kut", "kdtb", "-1");
    UdlSdrTblDvTbl("tuttbl");
}


function UdlSdrTblDvTbl(id) {//удаляем содержимое таблицы внутри какогото div с id
    //   var elem = document.getElementById("tblkfisp");
    //   elem.parentNode.removeChild(elem);
    var elm = document.getElementById(id);//"kut");
  //  var vt = elm.getElementsByTagName("table");
  //  say(vt);
    //  if (vt != null) {
    if (elm != null) {
        // var vtbl = vt[0];
        var vtbl = elm;
        while (vtbl.lastChild) {
            vtbl.removeChild(vtbl.lastChild);
        }
    //    say(vtbl);
    }
}//function UdlSdrTblDvTbl(id)


function vstvkslct(nzvid, klstlb, shpk) {
    var x = document.getElementById(nzvid);//"otrz");
  //  say(x);
    for (var i = 0; i < klstlb; i++) {
        var stt = "потолок " + shpk[i] + " стены " + shpk[klstlb + i] + " пол " + shpk[klstlb * 2 + (i)];
        var val = i + 1;
        var option = document.createElement("option");
        option.text = stt;
        option.value = val;
        if (i==0)
            option.selected = "selected";
        x.appendChild(option);
    //    x.add(option);
    //    say(x);
    }
}


function udlkofotrz() {//удаление данных в выборе отражения
    udlslctoptns("otrz");
}

function udlslctoptns(sid) {//удаление  options в select
    // var elm = document.getElementById("otrz");
    var elm = document.getElementById(sid);
    say("udlslctoptns");
    say(elm);
    while (elm.lastChild) {
        elm.removeChild(elm.lastChild);
    }
  //  say(elm);
}

function vstvtxtid(nzvid, dntxt) {
    var elmid = document.getElementById(nzvid);
    say(elmid);
    if (elmid != null) {
        elmid.value = dntxt;
    }
}

function udltlmpstrnc() {//удаляем информацию о лампе в светильнике
    // alert("udltlmpstrnc");
    //say('udltlmpstrnc()');
    var elms = ['nzvlmp', 'svtptk', 'pwr', 'kl'];
    inptclrponame(elms);
}

function inptclrponame(elms) {//очищаем поля заданных input полей
    if (elms instanceof Array) {
        var dl = elms.length;
        var index;
        for (index = 0; index < elms.length; ++index) {
            var elm = document.getElementsByName(elms[index]);
            elm[0].value = "";
            say(elm);
        }
    }
}


function zprvklsubmit(nzvid, status) {// блокирование и разблокирование кнопки submit
    var sbmt = document.getElementById(nzvid);
    if (sbmt.type == 'submit') {
        if (status == 'disabled') {
            sbmt.setAttribute('disabled', 'true');
        }
        else {
            var atr = sbmt.getAttribute('disabled');
            if (atr) {
                sbmt.removeAttribute('disabled');
            }
        }

    }
}


function fncrscht(e) {
    // Запрещаем стандартное поведение для кнопки submit
       // alert('submit');
    e.preventDefault();
    e.stopPropagation();
    udlrscht();
    var dtotpr = fncsbmtrscht();
       $.ajax({
        url: '/Home/RschtOsvet',
        type: "POST",
        dataType: "JSON",
        data: dtotpr,
        success: scsajaxrscht,
        complete: function () {// alert("Завершение выполнения"); 
        }
    });

}

function udlrscht()
{
    var elm = document.getElementById("rschtvyv");
   // alert("udlrscht");
    say(elm);
    while (elm.lastChild) {
        elm.removeChild(elm.lastChild);
    }
    say(elm);
}


function fncsbmtrscht() {
    var elfrm = document.getElementById('frmrscht');
    say(elfrm);
    var elms = ['dlnpom', 'shrpom', 'vyspom', 'nrmosv', 'urrbpov', 'vstsvs', 'kfzps'];
    var elms2 = ['kdtb', 'svtptk', 'nzvlmp', 'pwr', 'kl'];
    var dtotpr = new Object();
    for (var index = 0; index < elms.length; ++index) {
        var stt = elms[index];
        say(stt);
        var vs = document.getElementsByName(stt)[0].value;
        var velmf = elfrm.elements[stt];
      // var vvelms = elfrm.getElementsByName('dlnpom');//[0].elements;
        say(velmf);
        var elmval = velmf.value;
        var ptrn = velmf.getAttribute("pattern");
        say(ptrn);
       // var val = velmf.getAttribute('value');//value может не быть указано
        var reg = new RegExp(ptrn)
        var us = reg.test(vs);
        if (!us)
        {
            velmf.focus();
            return;
        }
        dtotpr[stt] = elmval;
       // alert(dtotpr);
    }
  //  alert(dtotpr);
    var vsv = document.getElementsByName('KdSvtl')[0].value;
    dtotpr['KdSvtl'] = vsv;
    var votr = document.getElementsByName('KdOtrz')[0].value;
    dtotpr['KdOtrz'] = votr;
    for (var index = 0; index < elms2.length; ++index) {
        var stt = elms2[index];
        var vs = document.getElementsByName(stt)[0].value;
        var velmf = elfrm.elements[stt];        
        say(velmf);
        var elmval = velmf.value;
        dtotpr[stt] = elmval;
    }
    return dtotpr;
}


function scsajaxrscht(data)
{
    //  alert("Завершение ")
    var elm = document.getElementById("rschtvyv");
    var newtext = document.createTextNode(data.Sitg);
    elm.appendChild(newtext);
}

