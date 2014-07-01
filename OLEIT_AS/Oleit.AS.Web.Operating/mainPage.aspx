<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainPage.aspx.cs" Inherits="Accounting_System.WebForm1" %>

<!DOCTYPE html> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account System Demo</title>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <link href="css/styles.css" rel="stylesheet" />
    <script type="text/javascript">
        function changePage(id) {
            if (id.indexOf("Logout.aspx")==-1) {
                $("#container").attr("src", id);
            } else {
                location.href = id;
            }
        }
    </script>
    <script type="text/javascript">
        
    </script>
    <style type="text/css">
    body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	background-image: url(img/MainBg.jpg);
    background-repeat: repeat-x;
	background-position: top;

	background-color: #272727;
    font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: 24px;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #FFF;
}
        #divFuncMenu ul,
        #divFuncMenu li,
        #divFuncMenu span,
        #divFuncMenu a
        {
            margin: 0;
            padding: 0;
            position: relative;
        }

        #divFuncMenu
        {
	height: 113px;
	width: 990px;
	background-image: url(img/menu_menu2.gif);
	background-repeat: no-repeat;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
        }

            #divFuncMenu:after,
            #divFuncMenu ul:after
            {
                content: '';
                display: block;
                clear: both;
            }



            #divFuncMenu ul
            {
	list-style: none;
	height: 26px;
	width: 680px;
	position: relative;
	left: 331px;
	top: 77px;
            }

            #divFuncMenu > ul
            {
                float: left;
            }

                #divFuncMenu > ul > li
                {
                    float: left;
                }

                    #divFuncMenu > ul > li > a
                    {
	color: #b1aba7;
	font-size: 12px;
	font-family: Verdana, Geneva, sans-serif;
	font-style: normal;
	line-height: normal;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	text-decoration: none;
	margin-right: 24px;
	margin-left: 24px;
                    }

/*                    #divFuncMenu > ul > li:hover:after
                    {
                        content: '';
                        display: block;
                        width: 0;
                        height: 0;
                        position: absolute;
                        left: 50%;
                        bottom: 0;
                        border-left: 10px solid transparent;
                        border-right: 10px solid transparent;
                        border-bottom: 10px solid #74a2db;
                        margin-left: -10px;
                    }*/



                    #divFuncMenu > ul > li.active:after
                    {
	content: '';
	display: block;
	width: 0;
	height: 0;
	position: absolute;
	left: 50%;
	bottom: 0;
	border-left: 10px solid transparent;
	border-right: 10px solid transparent;
	border-bottom: 10px solid #74a2db;
	margin-left: -10px;
	cursor: pointer;
                    }


                    #divFuncMenu > ul > li:hover > a
                    {
	color: #F60;
	cursor: pointer;
                    }

            #divFuncMenu .has-sub
            {
                z-index: 1;
            }

                #divFuncMenu .has-sub:hover > ul
                {
                    display: block;
                }

                #divFuncMenu .has-sub ul
                {
	display: none;
	position: absolute;
	width: 190px;
	top: 100%;
	left: 0;
	text-align: left;
                }



                        #divFuncMenu .has-sub ul li a
                        {
	filter: none;
	font-size: 12px;
	display: block;
	line-height: normal;
	padding: 10px;
	color: #4b4a4a;
	background-image: url(img/liMenu.png);
	font-family: Arial, Helvetica, sans-serif;
	font-style: normal;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	text-decoration: none;
                        }

                        #divFuncMenu .has-sub ul li:hover a
                        {
	color: #f6ae21;
	background-image: url(img/liMenu_on.gif);
	font-weight: bold;
                        }

                #divFuncMenu .has-sub .has-sub:hover > ul
                {
                    display: block;
                }



                    #divFuncMenu .has-sub .has-sub ul li a
                    {
                        background: #4b87d1;
                        border-bottom: 1px dotted #9dbde5;
                    }

                        #divFuncMenu .has-sub .has-sub ul li a:hover
                        {
                            background: #3779cb;
                        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <asp:Literal ID="ltrFuncMenu" runat="server">
                        </asp:Literal>
                    </td>
                </tr>
                <tr>
                    
                    <td style="height: 825px; text-align: center">
                        <iframe clientidmode="Static" id="container" class="containerIframe" src="Home.aspx?menuid=99"></iframe>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
