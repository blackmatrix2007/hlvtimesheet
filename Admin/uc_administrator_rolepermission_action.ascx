<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_rolepermission_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_rolepermission_action" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Information of role permission <%} else if (language.Equals("2")) { %> Thông tin nhóm quyền <%} %> </b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12">
	
        <div class="row">
            <div class="col-xs-12"> 
                <div id='control01' >
                    <a class="btn btn-primary" href="Administrator.aspx?func=91">
                        <span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")){ %>Quay lại<%} %>
                    </a>
                    <a class="btn btn-primary" href="javascript:LuuThongTin();">
                        <span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %> 
                    </a>
                </div>

               <div id='loading01' class="box box-danger box-solid" style='display:none;' >
                    <div class="box-header">
                        <h3 class="box-title">
                            <%if (language.Equals("1")){ %> Processing <%} else if (language.Equals("2")){ %> Đang xử lý <%} %></h3>
                    </div>
                    <div class="box-body">
                        <%if (language.Equals("1")){ %> The system is processing your request. Please wait a moment. <%}
                       else if (language.Equals("2")){ %> Hệ thống đang xử lý yêu cầu. Vui lòng chờ đợi trong giây lát. <%} %>
                        
                    </div>
                    <div class="overlay">
                        <i class="fa fa-refresh fa-spin"></i>
                    </div>
                </div><!-- /.box -->
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-12">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Code(*) <%} else if (language.Equals("2")) { %> Mã (*) <%} %> </div>
            <div class="col-xs-4"> 
                <input type="text" class="form-control" id="txtMa" runat="server" >
                <input type="hidden" id="cnqId" name="cnqId" value="<%= id %>" /> 
            </div>

            <div class="col-xs-2"> <input type="checkbox" id='chkTrangThai' runat="server" /> <i><%if (language.Equals("1")){ %> Action <%} else if (language.Equals("2")) { %> Hoạt động <%} %> </i> </div>
            <div class="col-xs-4"> 
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Description(*) <%} else if (language.Equals("2")) { %> Mô tả <%} %> </div>
            <div class="col-xs-10"> 
               <input type="text" class="form-control" id="txtTen" runat="server" >
            </div>
        </div>


        <div class="row" style="margin-top:10px;">
            <div class="col-xs-12">
                 <table class="table table-hover table-bordered table-striped" style="font-size:small">
                    <tr>
                        <th style="width:30%; text-align:center;">Function</th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> View / Print <%} else if (language.Equals("2")) { %> Xem / In <%} %> (<input type="checkbox" name="chkALL_XEM" onchange="chonHet(this, 'xemIds')" />) </th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> Delete <%} else if (language.Equals("2")) { %> Xóa <%} %>(<input type="checkbox" name="chkALL_XOA" onchange="chonHet(this, 'xoaIds')" />) </th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> Create <%} else if (language.Equals("2")) { %> Tạo mới <%} %>(<input type="checkbox" name="chkALL_TAOMOI" onchange="chonHet(this, 'taomoiIds')" />) </th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> Update <%} else if (language.Equals("2")) { %> Cập nhật <%} %>(<input type="checkbox" name="chkALL_CAPNHAT" onchange="chonHet(this, 'capnhatIds')" />)</th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> Approve <%} else if (language.Equals("2")) { %> Duyệt <%} %>(<input type="checkbox" name="chkALL_CHOT" onchange="chonHet(this, 'chotIds')" />)</th>
                        <th style="width:10%; text-align:center;"><%if (language.Equals("1")){ %> Cannel <%} else if (language.Equals("2")) { %> Hủy <%} %>(<input type="checkbox" name="chkALL_HUYCHOT" onchange="chonHet(this, 'huychotIds')" />)</th>
                    </tr>

                    <asp:Literal ID="ltQuyen_CN" runat="server"></asp:Literal>

                </table>
            </div>
        </div>        
   </div><!-- /.box-body -->  

    </div>
</div><!-- /.box -->

<script type="text/javascript">

    function chonHetGroup(e, name) {

        var khIds = document.getElementsByName(name);

        if (e.checked == true) {

            for (var i = 0; i < khIds.length; i++) {

                khIds.item(i).checked = true;
            }
        }
        else {

            for (var i = 0; i < khIds.length; i++) {

                khIds.item(i).checked = false;
            }
        }

    }

    function chonHet(e, name) {

        var groupId = document.getElementsByName("groupIds");
        //alert('GROUP ID: ' + groupId.length);

        for (var i = 0; i < groupId.length; i++) {

            //SELECT ALL GROUP LON
            var groupIds = document.getElementsByName(groupId.item(i).value + '_GR' + name);
            if (e.checked == true) {

                for (var j = 0; j < groupIds.length; j++) {

                    groupIds.item(j).checked = true;
                }
            }
            else {

                for (var j = 0; j < groupIds.length; j++) {

                    groupIds.item(j).checked = false;
                }
            }


            //SELECT ALL UNG DUNG CHI TIET
            var khIds = document.getElementsByName(groupId.item(i).value + '_' + name);
            if (e.checked == true) {

                for (var j = 0; j < khIds.length; j++) {

                    khIds.item(j).checked = true;
                }
            }
            else {

                for (var j = 0; j < khIds.length; j++) {

                    khIds.item(j).checked = false;
                }
            }

        }

    }

    function LuuThongTin() {

        var id = document.getElementById("cnqId").value;
        var maquyen = document.getElementById("<%= txtMa.ClientID %>").value;
        var diengiai = document.getElementById("<%= txtTen.ClientID %>").value;
        var trangthai = "0";
        if (document.getElementById("<%= chkTrangThai.ClientID %>").checked == true)
            trangthai = 1;

        if (maquyen == '') {
            alert('Please you must be intput code');
            return;
        }
        if (diengiai == '') {
            alert('Please you must be intput description');
            return;
        }

        var groupXEMSelected = '';
        var groupXOASelected = '';
        var groupTAOMOISelected = '';
        var groupCAPNHATSelected = '';
        var groupCHOTSelected = '';
        var groupHUYCHOTSelected = '';

        var xemSelected = '';
        var xoaSelected = '';
        var taomoiSelected = '';
        var capnhatSelected = '';
        var chotSelected = '';
        var huychotSelected = '';

        var groupId = document.getElementsByName("groupIds");

        for (var i = 0; i < groupId.length; i++) {

            //SELECT ALL GROUP LON
            var groupXemIds = document.getElementsByName(groupId.item(i).value + '_GRxemIds');
            for (var j = 0; j < groupXemIds.length; j++) {

                if (groupXemIds.item(j).checked)
                    groupXEMSelected += groupXemIds.item(j).value + ',';
            }

            var groupXoaIds = document.getElementsByName(groupId.item(i).value + '_GRxoaIds');
            for (var j = 0; j < groupXoaIds.length; j++) {

                if (groupXoaIds.item(j).checked)
                    groupXOASelected += groupXoaIds.item(j).value + ',';
            }

            var groupTaoMoiIds = document.getElementsByName(groupId.item(i).value + '_GRtaomoiIds');
            for (var j = 0; j < groupTaoMoiIds.length; j++) {

                if (groupTaoMoiIds.item(j).checked)
                    groupTAOMOISelected += groupTaoMoiIds.item(j).value + ',';
            }

            var groupCapNhatIds = document.getElementsByName(groupId.item(i).value + '_GRcapnhatIds');
            for (var j = 0; j < groupCapNhatIds.length; j++) {

                if (groupCapNhatIds.item(j).checked)
                    groupCAPNHATSelected += groupCapNhatIds.item(j).value + ',';
            }

            var groupChotIds = document.getElementsByName(groupId.item(i).value + '_GRchotIds');
            for (var j = 0; j < groupChotIds.length; j++) {

                if (groupChotIds.item(j).checked)
                    groupCHOTSelected += groupChotIds.item(j).value + ',';
            }

            var groupHuyChotIds = document.getElementsByName(groupId.item(i).value + '_GRhuychotIds');
            for (var j = 0; j < groupHuyChotIds.length; j++) {

                if (groupHuyChotIds.item(j).checked)
                    groupHUYCHOTSelected += groupHuyChotIds.item(j).value + ',';
            }

            //SELECT ALL UNG DUNG CHI TIET
            var ungdungXemIds = document.getElementsByName(groupId.item(i).value + '_xemIds');
            for (var j = 0; j < ungdungXemIds.length; j++) {

                if (ungdungXemIds.item(j).checked)
                    xemSelected += ungdungXemIds.item(j).value + ',';
            }

            var ungdungXoaIds = document.getElementsByName(groupId.item(i).value + '_xoaIds');
            for (var j = 0; j < ungdungXoaIds.length; j++) {

                if (ungdungXoaIds.item(j).checked)
                    xoaSelected += ungdungXoaIds.item(j).value + ',';
            }

            var ungdungTaoMoiIds = document.getElementsByName(groupId.item(i).value + '_taomoiIds');
            for (var j = 0; j < ungdungTaoMoiIds.length; j++) {

                if (ungdungTaoMoiIds.item(j).checked)
                    taomoiSelected += ungdungTaoMoiIds.item(j).value + ',';
            }

            var ungdungCapNhatIds = document.getElementsByName(groupId.item(i).value + '_capnhatIds');
            for (var j = 0; j < ungdungCapNhatIds.length; j++) {

                if (ungdungCapNhatIds.item(j).checked)
                    capnhatSelected += ungdungCapNhatIds.item(j).value + ',';
            }

            var ungdungChotIds = document.getElementsByName(groupId.item(i).value + '_chotIds');
            for (var j = 0; j < ungdungChotIds.length; j++) {

                if (ungdungChotIds.item(j).checked)
                    chotSelected += ungdungChotIds.item(j).value + ',';
            }

            var ungdungHuyChotIds = document.getElementsByName(groupId.item(i).value + '_huychotIds');
            for (var j = 0; j < ungdungHuyChotIds.length; j++) {

                if (ungdungHuyChotIds.item(j).checked)
                    huychotSelected += ungdungHuyChotIds.item(j).value + ',';
            }
        }

        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        $.post("Hander/hdPhanquyen.ashx?type=updateQuyen&id=" + id + "&ma=" + maquyen + "&diengiai=" + diengiai + "&trangthai=" + trangthai +
                "&groupXEMSelected=" + groupXEMSelected + "&groupXOASelected=" + groupXOASelected + "&groupTAOMOISelected=" + groupTAOMOISelected + "&groupCAPNHATSelected=" + groupCAPNHATSelected + "&groupCHOTSelected=" + groupCHOTSelected + "&groupHUYCHOTSelected=" + groupHUYCHOTSelected +
                "&xemSelected=" + xemSelected + "&xoaSelected=" + xoaSelected + "&taomoiSelected=" + taomoiSelected + "&capnhatSelected=" + capnhatSelected + "&chotSelected=" + chotSelected + "&huychotSelected=" + huychotSelected,
        function (result) {

            if (result.length <= 10) {

                alert('Save successfully');
                location.replace("Administrator.aspx?func=91");
            }
            else {

                divControl.style.display = '';
                divLoading.style.display = 'none';

                alert('Error! Cannot updated this permission. Please check again! \n ' + result);
            }

        });

    }

</script>


</section><!-- /.content -->

