
Partial Class Search
    Inherits System.Web.UI.Page
    Dim BTree As Node


    Protected Sub ButtonSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Dim Output() As String
        Dim SearchInput As String
        Dim OutputString() As String
        Dim PropositionList() As String
        Dim function_obj As New Functions
        'Dim BTree As Node
        Try
            SearchInput = TextBoxSearch.Text
            BTree = Session("BTree")
            PropositionList = Session("PropositionList")
            Output = function_obj.Parsing(SearchInput, PropositionList)

            OutputString = function_obj.Output(BTree, Output)
            Array.Sort(OutputString)
            Array.Reverse(OutputString)

            AssignParameters(OutputString, SearchInput)

            Dim Path As String
            Path = "Search.aspx"
            Response.Redirect(Path, False)

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim OutputPages As String
        Dim FilePath As String
        Dim OutputAllPages() As String
        Dim PageLists As ArrayList
        Dim UpperLimitPages As Integer
        Try
            FilePath = "Files/Input/"

            OutputAllPages = Session("Output")
            PageLists = Session("PageLists")

            If OutputAllPages.Length = 0 Then
                Response.Write("<BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR>")
                Response.Write("<B> NO DATA FOUND </B>")
            End If

            If Not IsPostBack Then
                DropDownListPages.DataSource = PageLists
                DropDownListPages.DataBind()
                TextBoxSearch.Text = Session("Input")
                BTree = Session("BTree")
            End If

            If PageLists.Count > 0 Then
                UpperLimitPages = (DropDownListPages.SelectedValue * 10)
                If UpperLimitPages > OutputAllPages.Length Then
                    UpperLimitPages = OutputAllPages.Length
                End If
                Response.Write("<BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR>")
                For Count As Integer = (DropDownListPages.SelectedValue - 1) * 10 To UpperLimitPages - 1
                    OutputPages = OutputAllPages(Count).Split("-")(1)
                    Response.Write("<BR><A TARGET=""_BLANK"" HREF=""" & FilePath & OutputPages & """>" & OutputPages & "</A>")
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub AssignParameters(ByVal OutputString() As String, ByVal SearchInput As String)
        Dim function_obj As New Functions
        Dim PageList As ArrayList
        Try
            Session("Output") = OutputString
            Session("Input") = SearchInput
            'Session("BTree") = Btree
            'Session("PropositionList") = PropositionList
            PageList = function_obj.ListofPageNos(OutputString)
            Session("PageLists") = PageList
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
End Class
