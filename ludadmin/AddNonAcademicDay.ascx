<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNonAcademicDay.ascx.cs" Inherits="OnlineLearningSystem.AddNonAcademicDay" %>
<div class="row">
    <div class="col-sm-5">
        <div class="add-poll-type">
            <h6>Add Non Academic Day</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Quarter: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListQuarter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarter_SelectedIndexChanged"></asp:DropDownList>

            <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Description: "></asp:Label>
            <asp:TextBox ID="TextBoxDescription" CssClass="form-control margin-bottom-10 font-size-13" runat="server" placeholder="" TextMode="MultiLine"></asp:TextBox>

            <asp:Label CssClass="sp-label" ID="lblType" runat="server" Text="Type: "></asp:Label>
            <%--<asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxType" runat="server"></asp:TextBox>--%>
            <div>
                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListType" runat="server">
                    <asp:ListItem Value="">--Select Type--</asp:ListItem>
                    <asp:ListItem Value="School">School</asp:ListItem>
                    <asp:ListItem Value="Holiday">Holiday</asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Start Date "></asp:Label>
            <div style="position: relative">
                <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxStartDate" placeholder="Start Date" runat="server"></asp:TextBox>
            </div>

            <asp:Label CssClass="sp-label" ID="Label17" runat="server" Text="End Date "></asp:Label>
            <div style="position: relative">
                <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxEndDate" placeholder="End Date" runat="server"></asp:TextBox>
            </div>

            <div>
                <asp:Button ID="btnAddNewNonAcademicDay" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Prerequisite" OnClick="btnAddNewNonAcademicDay_Click" />
            </div>
            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-7">
        <div class="margin-left-30">

            <div class="row">
                <div class="col-md-8">
                    <h6>Non Academic Day List</h6>
                </div>
                <div class="col-md-4 text-right">
                    <asp:Button ID="ClearAddNonAcademicDay" runat="server" CssClass="btn btn-sm btn-custom-header" Text="Clear All" OnClick="ClearAddNonAcademicDay_Click" />
                </div>
            </div>
            <hr style="margin-top: 5px" />
            <div class="custome-overflow">
                <div class="font-size-13">
                    <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" CssClass="custome-textarea form-control" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>

                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type">
                                <EditItemTemplate>
                                    <%--<asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Type") %>'></asp:TextBox>--%>
                                    <asp:DropDownList CssClass="form-control font-size-13 width-130 textbox-width" ID="DropDownListTypeGV" runat="server"></asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StartDate">
                                <EditItemTemplate>
                                    <div style="position: relative">
                                        <asp:TextBox ID="TextBox2" CssClass="tex-box form-control DatePicker input-text-fix-160" runat="server" Text='<%# Eval("StartDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                    </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("StartDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EndTime">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" CssClass="tex-box form-control DatePicker input-text-fix-160" runat="server" Text='<%# Eval("EndTime", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("EndTime", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TypeValue" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>
