
using Microsoft.AspNetCore.Html;

using System.Text;

namespace Microsoft.AspNetCore.Mvc.Rendering {
    public static class HtmlExtensions
    {


        public static HtmlString Pagination(this IHtmlHelper helper, int page, int total, int row, string getP = "")
        {

            int lastPage = (total + row - 1) / row;

            int prev = page - 1;
            int next = page + 1;


            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class=\"pagination\">");
            if (prev < 1)
                sb.Append("<li class=\"prev previous_page disabled\"><a rel=\"prev\" href=\"#\">Previous</a></li>");
            else
                sb.Append("<li class=\"prev previous_page\"><a rel=\"prev\" href=\"?page=" + prev + getP + "\">Previous</a></li>");


            for (int i = 1; i < 4; i++)
            {
                if (i == page)
                    sb.Append(PaginationLi(i, lastPage, true, getP));
                else
                    sb.Append(PaginationLi(i, lastPage, false, getP));

            }
            if (page > 6)
                sb.Append("<li class=\"disabled\"><a href=\"#\">…</a></li>");

            for (int i = (page - 2) < 4 ? 4 : (page - 2); i < page + 3; i++)
            {

                if (i == page)
                    sb.Append(PaginationLi(i, lastPage, true, getP));
                else
                    sb.Append(PaginationLi(i, lastPage, false, getP));

            }

            if (page + 2 < lastPage - 1)
            {
                sb.Append("<li class=\"disabled\"><a href=\"#\">…</a></li>");


                sb.Append(PaginationLi(lastPage - 1));
                sb.Append(PaginationLi(lastPage));
            }

            if (next > lastPage)
                sb.Append("<li class=\"next next_page disabled\"><a rel=\"next\" href=\"#\">Next</a></li>");
            else
                sb.Append("<li class=\"next next_page\"><a rel=\"next\" href=\"?page=" + next + getP + "\">Next</a></li>");


            sb.Append(" </ul>");

            return new HtmlString(sb.ToString());
        }

   

        private static string PaginationLi(int pg, bool active = false, string getP = "")
        {
            if (active)
                return "<li class=\"active\"><a href=\"?page=" + pg + getP + "\">" + pg + "</a></li>";
            else
                return "<li><a href=\"?page=" + pg + getP + "\">" + pg + "</a></li>";
        }

        private static string PaginationLi(int pg, int lastPage, bool active = false, string getP = "")
        {
            if (lastPage >= pg)
                return PaginationLi(pg, active, getP);
            return "";
        }







    }


}