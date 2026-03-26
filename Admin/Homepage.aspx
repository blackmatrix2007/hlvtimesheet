<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Diagram.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="HLVTimeSheet.Admin.Homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="box box-primary">

        <input type="hidden" id="language" name="language" value="<%= language %>" />        
        <input type="hidden" id="token" name="token" value="<%= token %>" />
        <input type="hidden" id="rolePC" name="rolePC" value="<%= rolePC %>" />
        <input type="hidden" id="roleOC" name="roleOC" value="<%= roleOC %>" />
        <input type="hidden" id="roleWS" name="roleWS" value="<%= roleWS %>" />
        <input type="hidden" id="roleVPW" name="roleVPW" value="<%= roleVPW %>" />
        <input type="hidden" id="roleHPBondedWH" name="roleHPW" value="<%= roleHPBondedWH %>" />
        <input type="hidden" id="roleHPNormalWH" name="roleHPW" value="<%= roleHPNormalWH %>" />
        <input type="hidden" id="roleSA" name="roleSA" value="<%= roleSA %>" />
        <input type="hidden" id="roleDW" name="roleDW" value="<%= roleDW %>" />
        <input type="hidden" id="roleJisseki" name="roleJisseki" value="<%= roleJisseki %>" />
        <input type="hidden" id="roleTimeSheet" name="roleTimeSheet" value="<%= roleTimeSheet %>" />

        <div class="box-body" id="nameSystem" style="font-size:x-small;">
            <div class="row" style="margin-top:5%; text-align:center;">                    
                <div class="col-xs-12 col-sm-12 col-lg-12">

                     <div class="col-lg-1"> </div>                   

                        <div class="col-xs-6 col-sm-3 col-lg-2 timeSheet" style="margin-top:10px; text-align:center; font-weight:bolder;">
                             <div>
                             <%if (roleTimeSheet.Equals("1"))
                                 { %>
                                 <a href="javascript:loadTimeSheet();"><img src="../Images/IconTimeSheet.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                             <%} else { %>
                                 <a><img src="../Images/IconTimeSheet.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                             <%} %>
                             </div>
                             <div>
                                 <%if (roleTimeSheet.Equals("1"))
                                 { %>
                                 <a href="javascript:loadTimeSheet();" style="color:black"><%= hlvTimeSheet  %></a>
                                 <%} else { %>
                                     <%= hlvTimeSheet  %>
                                 <%} %>         
                             </div>
                         </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 workSchedule" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>
                            <%if (roleWS.Equals("1"))
                                { %>
                                <a href="javascript:loadWorkScheduleSystem()"><img src="../Images/IconWorkSchedule.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} else { %>
                                <a><img src="../Images/IconWorkSchedule.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} %>   
                              </div>
                            <div>
                                 <%if (roleWS.Equals("1"))
                                { %>
                                <a href="javascript:loadWorkScheduleSystem();" style="color:black"><%= hlvWorkSchedule %></a>
                                <%} else { %>
                                    <%= hlvWorkSchedule %>
                                <%} %> 
                            </div>
                        </div>     
                    
                        <div class="col-xs-6 col-sm-3 col-lg-2 staffingArrangement" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>                        
                                <%if (roleSA.Equals("1"))
                                    { %>
                                    <a href="javascript:loadStaffingArrangement()"><img src="../Images/IconStaffingArrangement.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} else { %>
                                    <a><img src="../Images/IconStaffingArrangement.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} %>  
                              </div>
                            <div>
                                 <%if (roleSA.Equals("1"))
                                { %>
                                <a href="javascript:loadStaffingArrangement();" style="color:black"><%= hlvStaffingArrangement %></a>
                                <%} else { %>
                                    <%= hlvStaffingArrangement %>
                                <%} %>    
                            </div>
                         </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 jisseki" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>                        
                                <%if (roleJisseki.Equals("1"))
                                    { %>
                                    <a href="javascript:loadJisseki()"><img src="../Images/IconLaborProductivity.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} else { %>
                                    <a><img src="../Images/IconLaborProductivity.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} %>  
                              </div>
                            <div>
                                 <%if (roleJisseki.Equals("1"))
                                { %>
                                <a href="javascript:loadJisseki();" style="color:black"><%= hlvJisseki %></a>
                                <%} else { %>
                                    <%= hlvJisseki %>
                                <%} %>    
                            </div>
                         </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 documentWarning" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>                        
                                <%if (roleDW.Equals("1"))
                                    { %>
                                    <a href="javascript:loadDocumentWarning()"><img src="../Images/IconDocumentWarning.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} else { %>
                                    <a><img src="../Images/IconDocumentWarning.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} %>  
                              </div>
                            <div>
                                 <%if (roleDW.Equals("1"))
                                { %>
                                <a href="javascript:loadDocumentWarning();" style="color:black"><%= hlvDocumentWarning %></a>
                                <%} else { %>
                                    <%= hlvDocumentWarning %>
                                <%} %>    
                            </div>
                         </div>

                    <div class="col-lg-1"> </div>

               </div>

                <div class="col-xs-12 col-sm-12 col-lg-12" style="margin-top:5%; text-align:center;">

                    <div class="col-lg-1"> </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 operationControl" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>
                            <%if (roleOC.Equals("1"))
                                { %>
                                <a href="javascript:loadOperationControlSystem();"><img src="../Images/IconOperationControl.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} else { %>
                                <a><img src="../Images/IconOperationControl.jpg" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} %>
                            </div>
                            <div>
                                <%if (roleOC.Equals("1"))
                                { %>
                                <a href="javascript:loadOperationControlSystem();" style="color:black"><%= hlvOperationControl  %></a>
                                <%} else { %>
                                    <%= hlvOperationControl  %>
                                <%} %>         
                            </div>
                        </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 parkingCar" style="margin-top:10px; text-align:center; font-weight:bolder;">
                                <div>
                                    <%if (rolePC.Equals("1"))
                                        { %>
                                        <a href="javascript:loadParkingCarSystem();"><img src="../Images/IconParkingCar.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                    <%} else { %>
                                        <a><img src="../Images/IconParkingCar.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                    <%} %>  
                                    </div> 
                            <div>
                                <%if (rolePC.Equals("1"))
                                    { %>
                                        <a href="javascript:loadParkingCarSystem();"  style="color:black"><%= hlvParkingCar %></a>
                                <%} else { %>
                                    <%= hlvParkingCar %>
                                <%} %>     
                            </div>
                            </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 warehouseVP" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>
                                <%if (roleVPW.Equals("1"))
                                    { %>
                                    <a href="javascript:loadWarehouseVP()"><img src="../Images/IconWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} else { %>
                                    <a><img src="../Images/IconWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} %>   
                                </div> 
                            <div>
                                    <%if (roleVPW.Equals("1"))
                                { %>
                                <a href="javascript:loadWarehouseVP();" style="color:black"><%= HLVWarehouseVP %></a>
                                <%} else { %>
                                    <%= HLVWarehouseVP %>
                                <%} %>  
                            </div>
                        </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 warehouseHP" style="margin-top:10px; text-align:center; font-weight:bolder;">
                            <div>
                                <%if (roleHPBondedWH.Equals("1"))
                                    { %>
                                    <a href="javascript:loadWarehouseHP()"><img src="../Images/IconHaiPhongWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} else { %>
                                    <a><img src="../Images/IconHaiPhongWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                                <%} %>   
                                </div> 
                            <div>
                                    <%if (roleHPBondedWH.Equals("1"))
                                { %>
                                <a href="javascript:loadWarehouseHP();" style="color:black"><%= HLVBondedWHHP %></a>
                                <%} else { %>
                                    <%= HLVBondedWHHP %>
                                <%} %>  
                            </div>
                        </div>

                        <div class="col-xs-6 col-sm-3 col-lg-2 normalWarehouseHP" style="margin-top:10px; text-align:center; font-weight:bolder;">
                        <div>
                            <%if (roleHPNormalWH.Equals("1"))
                                { %>
                                <a href="javascript:loadNormalWarehouseHP()"><img src="../Images/IconHPNormalWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} else { %>
                                <a><img src="../Images/IconHPNormalWarehouse.png" style="width:80%; height:80%; border-radius: 50%;"/></a>
                            <%} %>   
                            </div> 
                        <div>
                                <%if (roleHPNormalWH.Equals("1"))
                            { %>
                            <a href="javascript:loadNormalWarehouseHP();" style="color:black"><%= HLVNormalWHHP %></a>
                            <%} else { %>
                                <%= HLVNormalWHHP %>
                            <%} %>  
                        </div>
                    </div>

                    <div class="col-lg-1"> </div>

                </div>
           </div>

            <div class="row" style="text-align:center;"> 
                <div class="col-xs-12 col-sm-12 col-lg-12"></div>
            </div>

        </div>

        <div class="footer" id="nameCompany">
            <div class="row">
            <div class="col-xs-12 col-sm-12 col-lg-12" style="margin-top:5%; text-align:center;"> 
                <a href="http://giangdc.company/" style="color:black;">
                    © <%= yearNow %>. The system developed by Giangdc Co.ltd
                </a>
            </div>
            </div>
        </div>

    </div>

<script type="text/javascript" >

    var language = document.getElementById("language").value;
    var token = document.getElementById("token").value;

    function loadParkingCarSystem() {
        window.open("http://hlv.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadOperationControlSystem() {
        window.open("http://hlv-oc.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadWorkScheduleSystem() {
        window.open("http://hlv-ws.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadWarehouseVP() {
        window.open("http://hlv-wh.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadWarehouseHP() {
        window.open("http://hlv-hp.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadNormalWarehouseHP() {
        window.open("http://hlv-hp2.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadStaffingArrangement() {
        window.open("http://hlv-ws.giangdc.company/Admin/StaffingArrangement.aspx?token=" + token + "&lang=" + language);
    }

    function loadJisseki() {
        window.open("http://hlv-jisseki.giangdc.company/Admin/LinkSystem.aspx?token=" + token + "&lang=" + language);
    }

    function loadDocumentWarning() {
        window.open("http://hlv-oc.giangdc.company/Admin/DocumentWarningShow.aspx?token=" + token + "&lang=" + language);
    }

    function loadTimeSheet() {
        window.open("http://hlv-ts.giangdc.company/Admin/LayoutMonthTimeSheet.aspx?token=" + token + "&lang=" + language);
    }

    function changeFontSizeFollowDevice() {
        // font-size:large;
        var sizeWidth = window.screen.width;

        if (parseFloat(sizeWidth) >= 1024) {
            $("#nameSystem").css({ "font-size": "medium" });
            $("#nameCompany").css({ "font-size": "small" });
        }
        else if (parseFloat(sizeWidth) < 1024 && parseFloat(sizeWidth) >= 500) {
            $("#nameSystem").css({ "font-size": "small" });
            $("#nameCompany").css({ "font-size": "smaller" });
        }
        else {
            $("#nameSystem").css({ "font-size": "smaller" });
            $("#nameCompany").css({ "font-size": "smaller" });
        }
    }

    $(document).ready(function () {

        changeFontSizeFollowDevice();

        var _rolePC = document.getElementById("rolePC").value;
        var _roleOC = document.getElementById("roleOC").value;
        var _roleWS = document.getElementById("roleWS").value;
        var _roleVPW = document.getElementById("roleVPW").value;
        var _roleHPBondedWH = document.getElementById("roleHPBondedWH").value;
        var _roleHPNormalWH = document.getElementById("roleHPNormalWH").value;
        var _roleSA = document.getElementById("roleSA").value;
        var _roleDW = document.getElementById("roleDW").value;
        var _roleJisseki = document.getElementById("roleJisseki").value;
        var _roleTimeSheet = document.getElementById("roleTimeSheet").value;

        if (_rolePC == "0")
            $('.parkingCar').css('color', 'LightGrey');
        if (_roleOC == "0")
            $('.operationControl').css('color', 'LightGrey');
        if (_roleWS == "0") 
            $('.workSchedule').css('color', 'LightGrey');
        if (_roleVPW == "0")
            $('.warehouseVP').css('color', 'LightGrey');
        if (_roleHPBondedWH == "0")
            $('.warehouseHP').css('color', 'LightGrey');
        if (_roleHPNormalWH == "0")
            $('.normalWarehouseHP').css('color', 'LightGrey');
        if (_roleSA == "0")
            $('.staffingArrangement').css('color', 'LightGrey');
        if (_roleDW == "0")
            $('.documentWarning').css('color', 'LightGrey');
        if (_roleJisseki == "0")
            $('.jisseki').css('color', 'LightGrey');
        if (_roleTimeSheet == "0")
            $('.timeSheet').css('color', 'LightGrey');

    });

</script>

</asp:Content>