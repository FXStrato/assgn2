using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trie
{
    ///
    /// This class implements a Trie datastructure to store words
    /// The Trie has Nodes and each node store its children nodes in a dictionary for fast retrieval
    /// Dictionary key is in ASCII Character type so there will be a limit of 26 items in the dictionary
    ///
    /// 
    public class JTrie : ITrie
    {
        /// if variable top in SearchPrefix not specified,
        /// the search result items will limits to this number
        /// 
        private int _maxSearchCount = 100;

        private Node _root;

        /// Init root node
        public JTrie()
        {
            _root = new Node() { NodeKey = ' ' };
        }

        /// max search count can be specified via the constructor
        /// else, default value will be used
        public JTrie(int maxSearchCount)
        {
            _root = new Node() { NodeKey = ' ' };
            _maxSearchCount = maxSearchCount;
        }

        public void Add(string str)
        {
            str = str.ToLower();
            Node cur = _root;
            Node tmp = null;
            foreach (char ch in str)
            {
                if (cur.Children == null)
                    cur.Children = new Dictionary<int, Node>();

                if (!cur.Children.Keys.Contains(ch))
                {
                    tmp = new Node() { NodeKey = ch };
                    cur.Children.Add(ch, tmp);
                }

                cur = cur.Children[ch];
                cur.NoOfPrefix += 1;
            }

            cur.IsWord = true;
        }

        /// search prefix return items with 'str' prefix
        /// 
        public List<string> SearchPrefix(string str, int top = -1)
        {
            List<string> result = new List<string>();
            Node cur = _root;
            string prefix = String.Empty;
            bool fail = false;

            foreach (char ch in str)
            {
                if (cur.Children == null)
                {
                    fail = true;
                    break;
                }

                if (cur.Children.Keys.Contains(ch))
                {
                    prefix += ch;
                    cur = cur.Children[ch];
                }
                else
                {
                    fail = true;
                    break;
                }
            }

            if (cur.IsWord && !fail && result.Count < top)
                result.Add(prefix);

            //from cur node, move down for more result
            top = (top == -1) ? _maxSearchCount : top;
            GetMoreWords(cur, result, prefix, top);

            return result;
        }

        public int GetPrefixCount(string str)
        {
            Node cur = _root;
            foreach (char ch in str)
            {
                if (cur.Children.Keys.Contains(ch))
                    cur = cur.Children[ch];
                else
                    return 0;
            }

            return cur.NoOfPrefix;
        }

        public bool Contains(string str)
        {
            bool contains = true;
            Node cur = _root;

            foreach (char ch in str)
            {
                if (cur.Children.Keys.Contains(ch))
                    cur = cur.Children[ch];
                else
                {
                    contains = false;
                    break;
                }
            }

            return contains;
        }

        /// recursive method iterates through all the trie and return full word with 
        /// specified prefix 
        /// 
        private void GetMoreWords(Node cur, List<string> result, string prefix, int top)
        {
            if (cur.Children == null)
                return;

            foreach (Node node in cur.Children.Values)
            {
                string tmp = prefix + node.NodeKey;
                if (node.IsWord)
                {
                    if (result.Count >= top)
                        break;
                    else
                        result.Add(tmp);

                }
                GetMoreWords(node, result, tmp, top);
            }
        }
    }
}