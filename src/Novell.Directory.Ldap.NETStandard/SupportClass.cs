/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
*
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to  permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/

//
// Novell.Directory.Ldap.SupportClass.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

// Support classes replicate the functionality of the original code, but in some cases they are
// substantially different architecturally. Although every effort is made to preserve the
// original architecture of the application in the converted project, the user should be aware that
// the primary goal of these support classes is to replicate functionality, and that at times
// the architecture of the resulting solution may differ somewhat.
//

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace Novell.Directory.Ldap
{
    /// <summary>
    ///     This interface should be implemented by any class whose instances are intended
    ///     to be executed by a thread.
    /// </summary>
    public interface IThreadRunnable
    {
        /// <summary>
        ///     This method has to be implemented in order that starting of the thread causes the object's
        ///     run method to be called in that separately executing thread.
        /// </summary>
        void Run();
    }

    /// <summary>
    ///     Contains conversion support elements such as classes, interfaces and static methods.
    /// </summary>
    public class SupportClass
    {
        /// <summary>
        ///     Converts a string to an array of bytes.
        /// </summary>
        /// <param name="sourceString">The string to be converted.</param>
        /// <returns>The new array of bytes.</returns>
        public static byte[] ToByteArray(string sourceString)
        {
            var byteArray = new byte[sourceString.Length];
            for (var index = 0; index < sourceString.Length; index++)
            {
                byteArray[index] = (byte)sourceString[index];
            }

            return byteArray;
        }

        /// <summary>
        ///     Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(object[] tempObjectArray)
        {
            var byteArray = new byte[tempObjectArray.Length];
            for (var index = 0; index < tempObjectArray.Length; index++)
            {
                byteArray[index] = (byte)tempObjectArray[index];
            }

            return byteArray;
        }

        /*******************************/

        /// <summary>
        ///     Reads a number of characters from the current source Stream and writes the data to the target array at the
        ///     specified index.
        /// </summary>
        /// <param name="sourceStream">The source Stream to read from.</param>
        /// <param name="target">Contains the array of characteres read from the source Stream.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source Stream.</param>
        /// <returns>
        ///     The number of characters read. The number will be less than or equal to count depending on the data available
        ///     in the source Stream. Returns -1 if the end of the stream is reached.
        /// </returns>
        public static int ReadInput(Stream sourceStream, ref byte[] target, int start, int count)
        {
            // Returns 0 bytes if not enough space in target
            if (target.Length == 0)
            {
                return 0;
            }

            var receiver = new byte[target.Length];
            var bytesRead = 0;
            var startIndex = start;
            var bytesToRead = count;
            while (bytesToRead > 0)
            {
                var n = sourceStream.Read(receiver, startIndex, bytesToRead);
                if (n == 0)
                {
                    break;
                }

                bytesRead += n;
                startIndex += n;
                bytesToRead -= n;
            }

            // Returns -1 if EOF
            if (bytesRead == 0)
            {
                return -1;
            }

            for (var i = start; i < start + bytesRead; i++)
            {
                target[i] = (byte)receiver[i];
            }

            return bytesRead;
        }

        /// <summary>
        ///     Reads a number of characters from the current source TextReader and writes the data to the target array at the
        ///     specified index.
        /// </summary>
        /// <param name="sourceTextReader">The source TextReader to read from.</param>
        /// <param name="target">Contains the array of characteres read from the source TextReader.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source TextReader.</param>
        /// <returns>
        ///     The number of characters read. The number will be less than or equal to count depending on the data available
        ///     in the source TextReader. Returns -1 if the end of the stream is reached.
        /// </returns>
        public static int ReadInput(TextReader sourceTextReader, ref byte[] target, int start, int count)
        {
            // Returns 0 bytes if not enough space in target
            if (target.Length == 0)
            {
                return 0;
            }

            var charArray = new char[target.Length];
            var bytesRead = sourceTextReader.Read(charArray, start, count);

            // Returns -1 if EOF
            if (bytesRead == 0)
            {
                return -1;
            }

            for (var index = start; index < start + bytesRead; index++)
            {
                target[index] = (byte)charArray[index];
            }

            return bytesRead;
        }

        /*******************************/

        /// <summary>
        ///     This method returns the literal value received.
        /// </summary>
        /// <param name="literal">The literal to return.</param>
        /// <returns>The received value.</returns>
        public static long Identity(long literal)
        {
            return literal;
        }

        /// <summary>
        ///     This method returns the literal value received.
        /// </summary>
        /// <param name="literal">The literal to return.</param>
        /// <returns>The received value.</returns>
        public static ulong Identity(ulong literal)
        {
            return literal;
        }

        /// <summary>
        ///     This method returns the literal value received.
        /// </summary>
        /// <param name="literal">The literal to return.</param>
        /// <returns>The received value.</returns>
        public static float Identity(float literal)
        {
            return literal;
        }

        /// <summary>
        ///     This method returns the literal value received.
        /// </summary>
        /// <param name="literal">The literal to return.</param>
        /// <returns>The received value.</returns>
        public static double Identity(double literal)
        {
            return literal;
        }

        /*******************************/

        /// <summary>
        ///     Gets the DateTimeFormat instance and date instance to obtain the date with the format passed.
        /// </summary>
        /// <param name="format">The DateTimeFormat to obtain the time and date pattern.</param>
        /// <param name="date">The date instance used to get the date.</param>
        /// <returns>A string representing the date with the time and date patterns.</returns>
        public static string FormatDateTime(DateTimeFormatInfo format, DateTime date)
        {
            var timePattern = DateTimeFormatManager.Manager.GetTimeFormatPattern(format);
            var datePattern = DateTimeFormatManager.Manager.GetDateFormatPattern(format);
            return date.ToString(datePattern + " " + timePattern, format);
        }

        /*******************************/

        /// <summary>
        ///     Adds a new key-and-value pair into the hash table.
        /// </summary>
        /// <param name="collection">The collection to work with.</param>
        /// <param name="key">Key used to obtain the value.</param>
        /// <param name="newValue">Value asociated with the key.</param>
        /// <returns>The old element associated with the key.</returns>
        public static object PutElement(IDictionary collection, object key, object newValue)
        {
            var element = collection[key];
            collection[key] = newValue;
            return element;
        }

        /*******************************/

        /// <summary>
        ///     Removes the first occurrence of an specific object from an ArrayList instance.
        /// </summary>
        /// <param name="arrayList">The ArrayList instance.</param>
        /// <param name="element">The element to remove.</param>
        /// <returns>True if item is found in the ArrayList; otherwise, false.</returns>
        public static bool VectorRemoveElement(ArrayList arrayList, object element)
        {
            var containsItem = arrayList.Contains(element);
            arrayList.Remove(element);
            return containsItem;
        }

        /*******************************/

        /// <summary>
        ///     Removes the element with the specified key from a Hashtable instance.
        /// </summary>
        /// <param name="hashtable">The Hashtable instance.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>The element removed.</returns>
        public static object HashtableRemove(Hashtable hashtable, object key)
        {
            var element = hashtable[key];
            hashtable.Remove(key);
            return element;
        }

        /*******************************/

        /// <summary>
        ///     Sets the size of the ArrayList. If the new size is greater than the current capacity, then new null items are added
        ///     to the end of the ArrayList. If the new size is lower than the current size, then all elements after the new size
        ///     are discarded.
        /// </summary>
        /// <param name="arrayList">The ArrayList to be changed.</param>
        /// <param name="newSize">The new ArrayList size.</param>
        public static void SetSize(ArrayList arrayList, int newSize)
        {
            if (newSize < 0)
            {
                throw new ArgumentException();
            }

            if (newSize < arrayList.Count)
            {
                arrayList.RemoveRange(newSize, arrayList.Count - newSize);
            }
            else
            {
                while (newSize > arrayList.Count)
                {
                    arrayList.Add(null);
                }
            }
        }

        /*******************************/

        /// <summary>
        ///     Adds an element to the top end of a Stack instance.
        /// </summary>
        /// <param name="stack">The Stack instance.</param>
        /// <param name="element">The element to add.</param>
        /// <returns>The element added.</returns>
        public static object StackPush(Stack stack, object element)
        {
            stack.Push(element);
            return element;
        }

        /*******************************/

        /// <summary>
        ///     Copies an array of chars obtained from a String into a specified array of chars.
        /// </summary>
        /// <param name="sourceString">The String to get the chars from.</param>
        /// <param name="sourceStart">Position of the String to start getting the chars.</param>
        /// <param name="sourceEnd">Position of the String to end getting the chars.</param>
        /// <param name="destinationArray">Array to return the chars.</param>
        /// <param name="destinationStart">Position of the destination array of chars to start storing the chars.</param>
        /// <returns>An array of chars.</returns>
        public static void GetCharsFromString(string sourceString, int sourceStart, int sourceEnd,
            ref char[] destinationArray, int destinationStart)
        {
            var sourceCounter = sourceStart;
            var destinationCounter = destinationStart;
            while (sourceCounter < sourceEnd)
            {
                destinationArray[destinationCounter] = sourceString[sourceCounter];
                sourceCounter++;
                destinationCounter++;
            }
        }

        /*******************************/

        /// <summary>
        ///     Creates an output file stream to write to the file with the specified name.
        /// </summary>
        /// <param name="fileName">Name of the file to write.</param>
        /// <param name="append">True in order to write to the end of the file, false otherwise.</param>
        /// <returns>New instance of FileStream with the proper file mode.</returns>
        public static FileStream GetFileStream(string fileName, bool append)
        {
            if (append)
            {
                return new FileStream(fileName, FileMode.Append);
            }

            return new FileStream(fileName, FileMode.Create);
        }

        /*******************************/

        /// <summary>
        ///     Converts an array of bytes to an array of chars.
        /// </summary>
        /// <param name="byteArray">The array of bytes to convert.</param>
        /// <returns>The new array of chars.</returns>
        public static char[] ToCharArray(byte[] byteArray)
        {
            var charArray = new char[byteArray.Length];
            byteArray.CopyTo(charArray, 0);
            return charArray;
        }

        /*******************************/

        /// <summary>
        ///     Creates an instance of a received Type.
        /// </summary>
        /// <param name="classType">The Type of the new class instance to return.</param>
        /// <returns>An Object containing the new instance.</returns>
        public static object CreateNewInstance(Type classType)
        {
            object instance = null;
            Type[] constructor = { };
            ConstructorInfo[] constructors = null;

            constructors = classType.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new UnauthorizedAccessException();
            }

            for (var i = 0; i < constructors.Length; i++)
            {
                var parameters = constructors[i].GetParameters();

                if (parameters.Length == 0)
                {
                    instance = classType.GetConstructor(constructor).Invoke(new object[] { });
                    break;
                }

                if (i == constructors.Length - 1)
                {
                    throw new MethodAccessException();
                }
            }

            return instance;
        }

        /*******************************/

        /// <summary>
        ///     Writes the exception stack trace to the received stream.
        /// </summary>
        /// <param name="throwable">Exception to obtain information from.</param>
        /// <param name="stream">Output sream used to write to.</param>
        public static void WriteStackTrace(Exception throwable, TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

        /*******************************/

        /// <summary>
        ///     Determines whether two Collections instances are equals.
        /// </summary>
        /// <param name="source">The first Collections to compare. </param>
        /// <param name="target">The second Collections to compare. </param>
        /// <returns>Return true if the first collection is the same instance as the second collection, otherwise return false.</returns>
        public static bool EqualsSupport(ICollection source, ICollection target)
        {
            var sourceEnumerator = ReverseStack(source);
            var targetEnumerator = ReverseStack(target);

            if (source.Count != target.Count)
            {
                return false;
            }

            while (sourceEnumerator.MoveNext() && targetEnumerator.MoveNext())
            {
                if (!sourceEnumerator.Current.Equals(targetEnumerator.Current))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Determines if a Collection is equal to the Object.
        /// </summary>
        /// <param name="source">The first Collections to compare.</param>
        /// <param name="target">The Object to compare.</param>
        /// <returns>Return true if the first collection contains the same values of the second Object, otherwise return false.</returns>
        public static bool EqualsSupport(ICollection source, object target)
        {
            if (target.GetType() != typeof(ICollection))
            {
                return false;
            }

            return EqualsSupport(source, (ICollection)target);
        }

        /// <summary>
        ///     Determines if a IDictionaryEnumerator is equal to the Object.
        /// </summary>
        /// <param name="source">The first IDictionaryEnumerator to compare.</param>
        /// <param name="target">The second Object to compare.</param>
        /// <returns>
        ///     Return true if the first IDictionaryEnumerator contains the same values of the second Object, otherwise return
        ///     false.
        /// </returns>
        public static bool EqualsSupport(IDictionaryEnumerator source, object target)
        {
            if (target.GetType() != typeof(IDictionaryEnumerator))
            {
                return false;
            }

            return EqualsSupport(source, (IDictionaryEnumerator)target);
        }

        /// <summary>
        ///     Determines whether two IDictionaryEnumerator instances are equals.
        /// </summary>
        /// <param name="source">The first IDictionaryEnumerator to compare.</param>
        /// <param name="target">The second IDictionaryEnumerator to compare.</param>
        /// <returns>
        ///     Return true if the first IDictionaryEnumerator contains the same values as the second IDictionaryEnumerator,
        ///     otherwise return false.
        /// </returns>
        public static bool EqualsSupport(IDictionaryEnumerator source, IDictionaryEnumerator target)
        {
            while (source.MoveNext() && target.MoveNext())
            {
                if (source.Key.Equals(target.Key))
                {
                    if (source.Value.Equals(target.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Reverses the Stack Collection received.
        /// </summary>
        /// <param name="collection">The collection to reverse.</param>
        /// <returns>
        ///     The collection received in reverse order if it was a System.Collections.Stack type, otherwise it does
        ///     nothing to the collection.
        /// </returns>
        public static IEnumerator ReverseStack(ICollection collection)
        {
            if (collection.GetType() == typeof(Stack))
            {
                var collectionStack = new ArrayList(collection);
                collectionStack.Reverse();
                return collectionStack.GetEnumerator();
            }

            return collection.GetEnumerator();
        }

        /*******************************/

        /// <summary>
        ///     The class performs token processing from strings.
        /// </summary>
        public class Tokenizer
        {
            private readonly bool _returnDelims;

            // The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character
            private string _delimiters = " \t\n\r";

            // Element list identified
            private ArrayList _elements;

            // Source string to use
            private string _source;

            /// <summary>
            ///     Initializes a new class instance with a specified string to process.
            /// </summary>
            /// <param name="source">String to tokenize.</param>
            public Tokenizer(string source)
            {
                _elements = new ArrayList();
                _elements.AddRange(source.Split(_delimiters.ToCharArray()));
                RemoveEmptyStrings();
                _source = source;
            }

            /// <summary>
            ///     Initializes a new class instance with a specified string to process
            ///     and the specified token delimiters to use.
            /// </summary>
            /// <param name="source">String to tokenize.</param>
            /// <param name="delimiters">String containing the delimiters.</param>
            public Tokenizer(string source, string delimiters)
            {
                _elements = new ArrayList();
                _delimiters = delimiters;
                _elements.AddRange(source.Split(_delimiters.ToCharArray()));
                RemoveEmptyStrings();
                _source = source;
            }

            public Tokenizer(string source, string delimiters, bool retDel)
            {
                _elements = new ArrayList();
                _delimiters = delimiters;
                _source = source;
                _returnDelims = retDel;
                if (_returnDelims)
                {
                    Tokenize();
                }
                else
                {
                    _elements.AddRange(source.Split(_delimiters.ToCharArray()));
                }

                RemoveEmptyStrings();
            }

            /// <summary>
            ///     Current token count for the source string.
            /// </summary>
            public int Count => _elements.Count;

            private void Tokenize()
            {
                var tempstr = _source;
                var toks = string.Empty;
                if (tempstr.IndexOfAny(_delimiters.ToCharArray()) < 0 && tempstr.Length > 0)
                {
                    _elements.Add(tempstr);
                }
                else if (tempstr.IndexOfAny(_delimiters.ToCharArray()) < 0 && tempstr.Length <= 0)
                {
                    return;
                }

                while (tempstr.IndexOfAny(_delimiters.ToCharArray()) >= 0)
                {
                    if (tempstr.IndexOfAny(_delimiters.ToCharArray()) == 0)
                    {
                        if (tempstr.Length > 1)
                        {
                            _elements.Add(tempstr.Substring(0, 1));
                            tempstr = tempstr.Substring(1);
                        }
                        else
                        {
                            tempstr = string.Empty;
                        }
                    }
                    else
                    {
                        toks = tempstr.Substring(0, tempstr.IndexOfAny(_delimiters.ToCharArray()));
                        _elements.Add(toks);
                        _elements.Add(tempstr.Substring(toks.Length, 1));
                        if (tempstr.Length > toks.Length + 1)
                        {
                            tempstr = tempstr.Substring(toks.Length + 1);
                        }
                        else
                        {
                            tempstr = string.Empty;
                        }
                    }
                }

                if (tempstr.Length > 0)
                {
                    _elements.Add(tempstr);
                }
            }

            /// <summary>
            ///     Determines if there are more tokens to return from the source string.
            /// </summary>
            /// <returns>True or false, depending if there are more tokens.</returns>
            public bool HasMoreTokens()
            {
                return _elements.Count > 0;
            }

            /// <summary>
            ///     Returns the next token from the token list.
            /// </summary>
            /// <returns>The string value of the token.</returns>
            public string NextToken()
            {
                string result;
                if (_source == string.Empty)
                {
                    throw new Exception();
                }

                if (_returnDelims)
                {
// Tokenize();
                    RemoveEmptyStrings();
                    result = (string)_elements[0];
                    _elements.RemoveAt(0);
                    return result;
                }

                _elements = new ArrayList();
                _elements.AddRange(_source.Split(_delimiters.ToCharArray()));
                RemoveEmptyStrings();
                result = (string)_elements[0];
                _elements.RemoveAt(0);
                _source = _source.Remove(_source.IndexOf(result), result.Length);
                _source = _source.TrimStart(_delimiters.ToCharArray());
                return result;
            }

            /// <summary>
            ///     Returns the next token from the source string, using the provided
            ///     token delimiters.
            /// </summary>
            /// <param name="delimiters">String containing the delimiters to use.</param>
            /// <returns>The string value of the token.</returns>
            public string NextToken(string delimiters)
            {
                _delimiters = delimiters;
                return NextToken();
            }

            /// <summary>
            ///     Removes all empty strings from the token list.
            /// </summary>
            private void RemoveEmptyStrings()
            {
                for (var index = 0; index < _elements.Count; index++)
                {
                    if ((string)_elements[index] == string.Empty)
                    {
                        _elements.RemoveAt(index);
                        index--;
                    }
                }
            }
        }

        /*******************************/

        /// <summary>
        ///     Provides support for DateFormat.
        /// </summary>
        public class DateTimeFormatManager
        {
            public static DateTimeFormatHashTable Manager = new DateTimeFormatHashTable();

            /// <summary>
            ///     Hashtable class to provide functionality for dateformat properties.
            /// </summary>
            public class DateTimeFormatHashTable : Hashtable
            {
                /// <summary>
                ///     Sets the format for datetime.
                /// </summary>
                /// <param name="format">DateTimeFormat instance to set the pattern.</param>
                /// <param name="newPattern">A string with the pattern format.</param>
                public void SetDateFormatPattern(DateTimeFormatInfo format, string newPattern)
                {
                    if (this[format] != null)
                    {
                        ((DateTimeFormatProperties)this[format]).DateFormatPattern = newPattern;
                    }
                    else
                    {
                        var tempProps = new DateTimeFormatProperties
                        {
                            DateFormatPattern = newPattern
                        };
                        Add(format, tempProps);
                    }
                }

                /// <summary>
                ///     Gets the current format pattern of the DateTimeFormat instance.
                /// </summary>
                /// <param name="format">The DateTimeFormat instance which the value will be obtained.</param>
                /// <returns>The string representing the current datetimeformat pattern.</returns>
                public string GetDateFormatPattern(DateTimeFormatInfo format)
                {
                    if (this[format] == null)
                    {
                        return "d-MMM-yy";
                    }

                    return ((DateTimeFormatProperties)this[format]).DateFormatPattern;
                }

                /// <summary>
                ///     Sets the datetimeformat pattern to the giving format.
                /// </summary>
                /// <param name="format">The datetimeformat instance to set.</param>
                /// <param name="newPattern">The new datetimeformat pattern.</param>
                public void SetTimeFormatPattern(DateTimeFormatInfo format, string newPattern)
                {
                    if (this[format] != null)
                    {
                        ((DateTimeFormatProperties)this[format]).TimeFormatPattern = newPattern;
                    }
                    else
                    {
                        var tempProps = new DateTimeFormatProperties
                        {
                            TimeFormatPattern = newPattern
                        };
                        Add(format, tempProps);
                    }
                }

                /// <summary>
                ///     Gets the current format pattern of the DateTimeFormat instance.
                /// </summary>
                /// <param name="format">The DateTimeFormat instance which the value will be obtained.</param>
                /// <returns>The string representing the current datetimeformat pattern.</returns>
                public string GetTimeFormatPattern(DateTimeFormatInfo format)
                {
                    if (this[format] == null)
                    {
                        return "h:mm:ss tt";
                    }

                    return ((DateTimeFormatProperties)this[format]).TimeFormatPattern;
                }

                /// <summary>
                ///     Internal class to provides the DateFormat and TimeFormat pattern properties on .NET.
                /// </summary>
                private class DateTimeFormatProperties
                {
                    public string DateFormatPattern = "d-MMM-yy";
                    public string TimeFormatPattern = "h:mm:ss tt";
                }
            }
        }

        /*******************************/

        /// <summary>
        ///     This class contains static methods to manage arrays.
        /// </summary>
        public class ArrayListSupport
        {
            /// <summary>
            ///     Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <param name="collection">The collection from wich to obtain the elements.</param>
            /// <param name="objects">The array containing all the elements of the collection.</param>
            /// <returns>The array containing all the elements of the collection.</returns>
            public static object[] ToArray(ArrayList collection, object[] objects)
            {
                var index = 0;
                var tempEnumerator = collection.GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    objects[index++] = tempEnumerator.Current;
                }

                return objects;
            }
        }

        /*******************************/

        /// <summary>
        ///     Support class used to handle threads.
        /// </summary>
        public class ThreadClass : IThreadRunnable
        {
            /// <summary>
            ///     The instance of System.Threading.Thread.
            /// </summary>
            private Thread _threadField;

            /// <summary>
            ///     Initializes a new instance of the ThreadClass class.
            /// </summary>
            public ThreadClass()
            {
                _threadField = new Thread(Run);
            }

            /// <summary>
            ///     Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="name">The name of the thread.</param>
            public ThreadClass(string name)
            {
                _threadField = new Thread(Run);
                Name = name;
            }

            /// <summary>
            ///     Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing.</param>
            public ThreadClass(ThreadStart start)
            {
                _threadField = new Thread(start);
            }

            /// <summary>
            ///     Initializes a new instance of the Thread class.
            /// </summary>
            /// <param name="start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing.</param>
            /// <param name="name">The name of the thread.</param>
            public ThreadClass(ThreadStart start, string name)
            {
                _threadField = new Thread(start);
                Name = name;
            }

            /// <summary>
            ///     Gets the current thread instance.
            /// </summary>
            public Thread Instance
            {
                get => _threadField;
                set => _threadField = value;
            }

            /// <summary>
            ///     Gets or sets the name of the thread.
            /// </summary>
            public string Name
            {
                get => _threadField.Name;
                set
                {
                    if (_threadField.Name == null)
                    {
                        _threadField.Name = value;
                    }
                }
            }

            /// <summary>
            ///     Gets a value indicating the execution status of the current thread.
            /// </summary>
            public bool IsAlive => _threadField.IsAlive;

            /// <summary>
            ///     Gets or sets a value indicating whether or not a thread is a background thread.
            /// </summary>
            public bool IsBackground
            {
                get => _threadField.IsBackground;
                set => _threadField.IsBackground = value;
            }

            public bool IsStopping { get; private set; }

            /// <summary>
            ///     This method has no functionality unless the method is overridden.
            /// </summary>
            public virtual void Run()
            {
            }

            /// <summary>
            ///     Causes the operating system to change the state of the current thread instance to ThreadState.Running.
            /// </summary>
            public void Start()
            {
                _threadField.Start();
            }

            ///// <summary>
            ///// Interrupts a thread that is in the WaitSleepJoin thread state
            ///// </summary>
            // public virtual void Interrupt()
            // {
            // threadField.Interrupt();
            // }

            public void Stop()
            {
                IsStopping = true;
            }

            /// <summary>
            ///     Blocks the calling thread until a thread terminates.
            /// </summary>
            public void Join()
            {
                _threadField.Join();
            }

            /// <summary>
            ///     Blocks the calling thread until a thread terminates or the specified time elapses.
            /// </summary>
            /// <param name="miliSeconds">Time of wait in milliseconds.</param>
            public void Join(int miliSeconds)
            {
                lock (this)
                {
                    _threadField.Join(miliSeconds * 10000);
                }
            }

            /// <summary>
            ///     Blocks the calling thread until a thread terminates or the specified time elapses.
            /// </summary>
            /// <param name="miliSeconds">Time of wait in milliseconds.</param>
            /// <param name="nanoSeconds">Time of wait in nanoseconds.</param>
            public void Join(int miliSeconds, int nanoSeconds)
            {
                lock (this)
                {
                    _threadField.Join(miliSeconds * 10000 + nanoSeconds * 100);
                }
            }

            /// <summary>
            ///     Obtain a String that represents the current Object.
            /// </summary>
            /// <returns>A String that represents the current Object.</returns>
            public override string ToString()
            {
                return "Thread[" + Name + "]";
            }

            /// <summary>
            ///     Gets the currently running thread.
            /// </summary>
            /// <returns>The currently running thread.</returns>
            public static ThreadClass Current()
            {
                var currentThread = new ThreadClass
                {
                    Instance = Thread.CurrentThread
                };
                return currentThread;
            }
        }

        /*******************************/

        /// <summary>
        ///     This class contains different methods to manage Collections.
        /// </summary>
        public class CollectionSupport : CollectionBase
        {
            /// <summary>
            ///     Adds an specified element to the collection.
            /// </summary>
            /// <param name="element">The element to be added.</param>
            /// <returns>Returns true if the element was successfuly added. Otherwise returns false.</returns>
            public bool Add(object element)
            {
                return List.Add(element) != -1;
            }

            /// <summary>
            ///     Adds all the elements contained in the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be added.</param>
            /// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
            public bool AddAll(ICollection collection)
            {
                var result = false;
                if (collection != null)
                {
                    var tempEnumerator = new ArrayList(collection).GetEnumerator();
                    while (tempEnumerator.MoveNext())
                    {
                        if (tempEnumerator.Current != null)
                        {
                            result = Add(tempEnumerator.Current);
                        }
                    }
                }

                return result;
            }

            /// <summary>
            ///     Adds all the elements contained in the specified support class collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be added.</param>
            /// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
            public bool AddAll(CollectionSupport collection)
            {
                return AddAll((ICollection)collection);
            }

            /// <summary>
            ///     Verifies if the specified element is contained into the collection.
            /// </summary>
            /// <param name="element"> The element that will be verified.</param>
            /// <returns>Returns true if the element is contained in the collection. Otherwise returns false.</returns>
            public bool Contains(object element)
            {
                return List.Contains(element);
            }

            /// <summary>
            ///     Verifies if all the elements of the specified collection are contained into the current collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be verified.</param>
            /// <returns>Returns true if all the elements are contained in the collection. Otherwise returns false.</returns>
            public bool ContainsAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = new ArrayList(collection).GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    if (!(result = Contains(tempEnumerator.Current)))
                    {
                        break;
                    }
                }

                return result;
            }

            /// <summary>
            ///     Verifies if all the elements of the specified collection are contained into the current collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be verified.</param>
            /// <returns>Returns true if all the elements are contained in the collection. Otherwise returns false.</returns>
            public bool ContainsAll(CollectionSupport collection)
            {
                return ContainsAll((ICollection)collection);
            }

            /// <summary>
            ///     Verifies if the collection is empty.
            /// </summary>
            /// <returns>Returns true if the collection is empty. Otherwise returns false.</returns>
            public bool IsEmpty()
            {
                return Count == 0;
            }

            /// <summary>
            ///     Removes an specified element from the collection.
            /// </summary>
            /// <param name="element">The element to be removed.</param>
            /// <returns>Returns true if the element was successfuly removed. Otherwise returns false.</returns>
            public bool Remove(object element)
            {
                var result = false;
                if (Contains(element))
                {
                    List.Remove(element);
                    result = true;
                }

                return result;
            }

            /// <summary>
            ///     Removes all the elements contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be removed.</param>
            /// <returns>Returns true if all the elements were successfuly removed. Otherwise returns false.</returns>
            public bool RemoveAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = new ArrayList(collection).GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    if (Contains(tempEnumerator.Current))
                    {
                        result = Remove(tempEnumerator.Current);
                    }
                }

                return result;
            }

            /// <summary>
            ///     Removes all the elements contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be removed.</param>
            /// <returns>Returns true if all the elements were successfuly removed. Otherwise returns false.</returns>
            public bool RemoveAll(CollectionSupport collection)
            {
                return RemoveAll((ICollection)collection);
            }

            /// <summary>
            ///     Removes all the elements that aren't contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to verify the elements that will be retained.</param>
            /// <returns>Returns true if all the elements were successfully removed. Otherwise returns false.</returns>
            public bool RetainAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = GetEnumerator();
                var tempCollection = new CollectionSupport();
                tempCollection.AddAll(collection);
                while (tempEnumerator.MoveNext())
                {
                    if (!tempCollection.Contains(tempEnumerator.Current))
                    {
                        result = Remove(tempEnumerator.Current);

                        if (result)
                        {
                            tempEnumerator = GetEnumerator();
                        }
                    }
                }

                return result;
            }

            /// <summary>
            ///     Removes all the elements that aren't contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to verify the elements that will be retained.</param>
            /// <returns>Returns true if all the elements were successfully removed. Otherwise returns false.</returns>
            public bool RetainAll(CollectionSupport collection)
            {
                return RetainAll((ICollection)collection);
            }

            /// <summary>
            ///     Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <returns>The array containing all the elements of the collection.</returns>
            public object[] ToArray()
            {
                var index = 0;
                var objects = new object[Count];
                var tempEnumerator = GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    objects[index++] = tempEnumerator.Current;
                }

                return objects;
            }

            /// <summary>
            ///     Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <param name="objects">The array into which the elements of the collection will be stored.</param>
            /// <returns>The array containing all the elements of the collection.</returns>
            public object[] ToArray(object[] objects)
            {
                var index = 0;
                var tempEnumerator = GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    objects[index++] = tempEnumerator.Current;
                }

                return objects;
            }

            /// <summary>
            ///     Creates a CollectionSupport object with the contents specified in array.
            /// </summary>
            /// <param name="array">The array containing the elements used to populate the new CollectionSupport object.</param>
            /// <returns>A CollectionSupport object populated with the contents of array.</returns>
            public static CollectionSupport ToCollectionSupport(object[] array)
            {
                var tempCollectionSupport = new CollectionSupport();
                tempCollectionSupport.AddAll(array);
                return tempCollectionSupport;
            }
        }

        /*******************************/

        /// <summary>
        ///     This class manages a set of elements.
        /// </summary>
        public class SetSupport : ArrayList
        {
            /// <summary>
            ///     Creates a new set.
            /// </summary>
            public SetSupport()
            {
            }

            /// <summary>
            ///     Creates a new set initialized with System.Collections.ICollection object.
            /// </summary>
            /// <param name="collection">System.Collections.ICollection object to initialize the set object.</param>
            public SetSupport(ICollection collection)
                : base(collection)
            {
            }

            /// <summary>
            ///     Creates a new set initialized with a specific capacity.
            /// </summary>
            /// <param name="capacity">value to set the capacity of the set object.</param>
            public SetSupport(int capacity)
                : base(capacity)
            {
            }

            /// <summary>
            ///     Adds an element to the set.
            /// </summary>
            /// <param name="objectToAdd">The object to be added.</param>
            /// <returns>True if the object was added, false otherwise.</returns>
            public new virtual bool Add(object objectToAdd)
            {
                if (Contains(objectToAdd))
                {
                    return false;
                }

                base.Add(objectToAdd);
                return true;
            }

            /// <summary>
            ///     Adds all the elements contained in the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be added.</param>
            /// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
            public virtual bool AddAll(ICollection collection)
            {
                var result = false;
                if (collection != null)
                {
                    var tempEnumerator = new ArrayList(collection).GetEnumerator();
                    while (tempEnumerator.MoveNext())
                    {
                        if (tempEnumerator.Current != null)
                        {
                            result = Add(tempEnumerator.Current);
                        }
                    }
                }

                return result;
            }

            /// <summary>
            ///     Adds all the elements contained in the specified support class collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be added.</param>
            /// <returns>Returns true if all the elements were successfuly added. Otherwise returns false.</returns>
            public bool AddAll(CollectionSupport collection)
            {
                return AddAll((ICollection)collection);
            }

            /// <summary>
            ///     Verifies that all the elements of the specified collection are contained into the current collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be verified.</param>
            /// <returns>True if the collection contains all the given elements.</returns>
            public bool ContainsAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = collection.GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    if (!(result = Contains(tempEnumerator.Current)))
                    {
                        break;
                    }
                }

                return result;
            }

            /// <summary>
            ///     Verifies if all the elements of the specified collection are contained into the current collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be verified.</param>
            /// <returns>Returns true if all the elements are contained in the collection. Otherwise returns false.</returns>
            public bool ContainsAll(CollectionSupport collection)
            {
                return ContainsAll((ICollection)collection);
            }

            /// <summary>
            ///     Verifies if the collection is empty.
            /// </summary>
            /// <returns>True if the collection is empty, false otherwise.</returns>
            public virtual bool IsEmpty()
            {
                return Count == 0;
            }

            /// <summary>
            ///     Removes an element from the set.
            /// </summary>
            /// <param name="elementToRemove">The element to be removed.</param>
            /// <returns>True if the element was removed.</returns>
            public new virtual bool Remove(object elementToRemove)
            {
                var result = false;
                if (Contains(elementToRemove))
                {
                    result = true;
                }

                base.Remove(elementToRemove);
                return result;
            }

            /// <summary>
            ///     Removes all the elements contained in the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be removed.</param>
            /// <returns>True if all the elements were successfuly removed, false otherwise.</returns>
            public bool RemoveAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = collection.GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    if (result == false && Contains(tempEnumerator.Current))
                    {
                        result = true;
                    }

                    Remove(tempEnumerator.Current);
                }

                return result;
            }

            /// <summary>
            ///     Removes all the elements contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to extract the elements that will be removed.</param>
            /// <returns>Returns true if all the elements were successfuly removed. Otherwise returns false.</returns>
            public bool RemoveAll(CollectionSupport collection)
            {
                return RemoveAll((ICollection)collection);
            }

            /// <summary>
            ///     Removes all the elements that aren't contained in the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to verify the elements that will be retained.</param>
            /// <returns>True if all the elements were successfully removed, false otherwise.</returns>
            public bool RetainAll(ICollection collection)
            {
                var result = false;
                var tempEnumerator = collection.GetEnumerator();
                var tempSet = (SetSupport)collection;
                while (tempEnumerator.MoveNext())
                {
                    if (!tempSet.Contains(tempEnumerator.Current))
                    {
                        result = Remove(tempEnumerator.Current);
                        tempEnumerator = GetEnumerator();
                    }
                }

                return result;
            }

            /// <summary>
            ///     Removes all the elements that aren't contained into the specified collection.
            /// </summary>
            /// <param name="collection">The collection used to verify the elements that will be retained.</param>
            /// <returns>Returns true if all the elements were successfully removed. Otherwise returns false.</returns>
            public bool RetainAll(CollectionSupport collection)
            {
                return RetainAll((ICollection)collection);
            }

            /// <summary>
            ///     Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <returns>The array containing all the elements of the collection.</returns>
            public new object[] ToArray()
            {
                var index = 0;
                var tempObject = new object[Count];
                var tempEnumerator = GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    tempObject[index++] = tempEnumerator.Current;
                }

                return tempObject;
            }

            /// <summary>
            ///     Obtains an array containing all the elements in the collection.
            /// </summary>
            /// <param name="objects">The array into which the elements of the collection will be stored.</param>
            /// <returns>The array containing all the elements of the collection.</returns>
            public object[] ToArray(object[] objects)
            {
                var index = 0;
                var tempEnumerator = GetEnumerator();
                while (tempEnumerator.MoveNext())
                {
                    objects[index++] = tempEnumerator.Current;
                }

                return objects;
            }
        }

        /*******************************/

        /// <summary>
        ///     This class manages different operation with collections.
        /// </summary>
        public class AbstractSetSupport : SetSupport
        {
        }
    }
}