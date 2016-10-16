$(document).ready(function () {
    
    //сразу установим ссылку
    var vvs = $("#TblKfId option:selected").val();
      
    $('#TblKfId').on('change', fncchangetblname);
    
    UstnPrhdSlctZnc(vvs);          
});

function UstnPrhdSlctZnc(vslct) {
 
  
     izmhrf("iskfot",vslct);
     izmhrf("isipm",vslct);
 }


//
function izmhrf(hid,znc)
{
    var elm = document.getElementById(hid);
    var hatrb = elm.getAttribute("href");
   
    var pos = hatrb.lastIndexOf("/");
    var res = hatrb.slice(0, pos + 1);
    
    var satr = res.concat(znc);
  
    elm.setAttribute("href", satr);
}


function fncchangetblname() {
    var val = this.value;
    UstnPrhdSlctZnc(val)
}

