
Partial Class _Default
    Inherits System.Web.UI.Page
    Protected Sub ButtonSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Dim SearchInput As String
        Dim Output() As String
        Dim OutputString() As String
        Dim PropositionList() As String
        Dim function_obj As New Functions
        Dim Searcher_obj As New Searcher
        Dim BTree As Node
        Try
            SearchInput = TextBoxSearch.Text
            PropositionList = function_obj.GetPropositionsList()
            Output = function_obj.Parsing(SearchInput, PropositionList)
            BTree = Searcher_obj.GetFileList()
            OutputString = function_obj.Output(BTree, Output)
            Array.Sort(OutputString)
            Array.Reverse(OutputString)

            If OutputString.Length <> 0 Then
                Dim Path As String
                AssignParameters(OutputString, SearchInput, BTree, PropositionList)
                Path = "Search.aspx"
                Response.Redirect(Path, False)
            Else
                Response.Write("<BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR><BR>")
                Response.Write("<B> NO DATA FOUND </B>")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Sub AssignParameters(ByVal OutputString() As String, ByVal SearchInput As String, ByVal Btree As Node, ByVal PropositionList() As String)
        Dim function_obj As New Functions
        Dim PageList As ArrayList
        Try
            Session("Output") = OutputString
            Session("Input") = SearchInput
            Session("BTree") = Btree
            Session("PropositionList") = PropositionList
            PageList = function_obj.ListofPageNos(OutputString)
            Session("PageLists") = PageList
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

End Class
