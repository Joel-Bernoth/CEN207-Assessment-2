using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEN207_Assessment_2.Structures
{
    public class FeedPost
    {

        private string userName;
        private string name;
        private string title;
        private string body;


        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (value.Any(c => c == ' '))
                    throw new Exception("Invalid name. (It cannot contain spaces)");

                this.userName = value;
            }
        }

        public string Name
        {
            get => this.userName;
            set => this.userName = value;
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
            }
        }
        public string Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
            }
        }
    }
}
