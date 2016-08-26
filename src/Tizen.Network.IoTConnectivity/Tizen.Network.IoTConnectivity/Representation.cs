/// Copyright 2016 by Samsung Electronics, Inc.,
///
/// This software is the confidential and proprietary information
/// of Samsung Electronics, Inc. ("Confidential Information"). You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with Samsung.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Tizen.Network.IoTConnectivity
{
    /// <summary>
    /// This class provides API to manage representation.
    /// A representation is a payload of a request or a response.
    /// </summary>
    public class Representation : IDisposable
    {
        internal IntPtr _representationHandle = IntPtr.Zero;

        private bool _disposed = false;
        private ObservableCollection<Representation> _children = new ObservableCollection<Representation>();

        /// <summary>
        /// The Representation constructor
        /// </summary>
        /// </summary>
        /// <code>
        /// Representation repr = new Representation();
        /// </code>
        public Representation()
        {
            int ret = Interop.IoTConnectivity.Common.Representation.Create(out _representationHandle);
            if (ret != (int)IoTConnectivityError.None)
            {
                Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to create representation");
                throw IoTConnectivityErrorFactory.GetException(ret);
            }

            _children.CollectionChanged += ChildrenCollectionChanged;
        }

        // Constructor for cloning native representation object
        internal Representation(IntPtr handleToClone)
        {
            int ret = (int)IoTConnectivityError.InvalidParameter;
            if (handleToClone != IntPtr.Zero)
            {
                ret = Interop.IoTConnectivity.Common.Representation.Clone(handleToClone, out _representationHandle);
            }
            if (ret != (int)IoTConnectivityError.None)
            {
                Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to create representation");
                throw IoTConnectivityErrorFactory.GetException(ret);
            }

            _children.CollectionChanged += ChildrenCollectionChanged;
        }

        /// <summary>
        /// Destructor of the Representation class.
        /// </summary>
        ~Representation()
        {
            Dispose(false);
        }

        /// <summary>
        /// The URI path of resource
        /// </summary>
        /// <remarks>
        /// Setter can throw exceptions
        /// </remarks>
        /// <code>
        /// Representation repr = new Representation();
        /// repr.UriPath = "/a/light";
        /// Console.WriteLine("URI is {0}", repr.UriPath);  //Getter
        /// </code>
        public string UriPath
        {
            get
            {
                IntPtr path;
                int ret = Interop.IoTConnectivity.Common.Representation.GetUriPath(_representationHandle, out path);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to Get uri");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
                return Marshal.PtrToStringAnsi(path);
            }
            set
            {
                int ret = (int)IoTConnectivityError.InvalidParameter;
                if (value != null)
                    ret = Interop.IoTConnectivity.Common.Representation.SetUriPath(_representationHandle, value);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to set uri");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
            }
        }

        /// <summary>
        /// The type of resource
        /// </summary>
        /// <seealso cref="ResourceTypes"/>
        /// <code>
        /// Representation repr = new Representation();
        /// ResourceTypes types = new ResourceTypes (new List<string>(){ "org.tizen.light" });
        /// repr.Type = types;
        /// var type = repr.Type;   // Getter
        /// foreach (string item in type)
        /// {
        ///     Console.WriteLine("Type is {0}", item);
        /// }
        /// </code>
        public ResourceTypes Type
        {
            get
            {
                IntPtr typeHandle;
                int ret = Interop.IoTConnectivity.Common.Representation.GetResourceTypes(_representationHandle, out typeHandle);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to get type");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
                if (typeHandle == IntPtr.Zero)
                {
                    return null;
                }
                return new ResourceTypes(typeHandle);
            }
            set
            {
                int ret = (int)IoTConnectivityError.InvalidParameter;
                if (value != null)
                    ret = Interop.IoTConnectivity.Common.Representation.SetResourceTypes(_representationHandle, value._resourceTypeHandle);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to set type");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
            }
        }

        /// <summary>
        /// The interface of the resource
        /// </summary>
        /// <seealso cref="ResourceInterfaces"/>
        /// <code>
        /// Representation repr = new Representation();
        /// ResourceInterfaces ifaces = new ResourceInterfaces (new List<string>(){ ResourceInterfaces.DefaultInterface });
        /// repr.Interface = ifaces;
        /// var iface = repr.Interface;   // Getter
        /// foreach (string item in iface)
        /// {
        ///     Console.WriteLine("Interface is {0}", iface);
        /// }
        /// </code>
        public ResourceInterfaces Interface
        {
            get
            {
                IntPtr interfaceHandle;
                int ret = Interop.IoTConnectivity.Common.Representation.GetResourceInterfaces(_representationHandle, out interfaceHandle);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to get interface");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
                if (interfaceHandle == IntPtr.Zero)
                {
                    return null;
                }
                return new ResourceInterfaces(interfaceHandle);
            }
            set
            {
                int ret = (int)IoTConnectivityError.InvalidParameter;
                if (value != null)
                    ret = Interop.IoTConnectivity.Common.Representation.SetResourceInterfaces(_representationHandle, value.ResourceInterfacesHandle);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to set interface");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
            }
        }

        /// <summary>
        /// Current attributes of the resource
        /// </summary>
        /// <seealso cref="Attributes"/>
        /// <code>
        /// Representation repr = new Representation();
        /// Attributes attributes = new Attributes() {
        ///     { "state", "ON" },
        ///     { "dim", 10 }
        /// };
        /// repr.Attributes = attributes;
        /// var newAttributes = repr.Attributes;   // Getter
        /// string strval = newAttributes["state"] as string;
        /// int intval = (int)newAttributes["dim"];
        /// Console.WriteLine("attributes are {0} and {1}", strval, intval);
        /// </code>
        public Attributes Attributes
        {
            get
            {
                IntPtr attributeHandle;
                int ret = Interop.IoTConnectivity.Common.Representation.GetAttributes(_representationHandle, out attributeHandle);
                if (ret != (int)IoTConnectivityError.None)
                {
                    Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to get attributes");
                    throw IoTConnectivityErrorFactory.GetException(ret);
                }
                return new Attributes(attributeHandle);
            }
            set
            {
                int ret = (int)IoTConnectivityError.InvalidParameter;
                if (value != null)
                {
                    ret = Interop.IoTConnectivity.Common.Representation.SetAttributes(_representationHandle, value._resourceAttributesHandle);
                    if (ret != (int)IoTConnectivityError.None)
                    {
                        Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to set attributes");
                        throw IoTConnectivityErrorFactory.GetException(ret);
                    }
                }
            }
        }

        /// <summary>
        /// List of Child resource representation
        /// </summary>
        /// <code>
        /// Representation repr = new Representation();
        /// Representation child1 = new Representation();
        /// ResourceTypes types1 = new ResourceTypes(new List<string>() { "org.tizen.light" });
        /// child1.Type = types1;
        /// ResourceInterfaces ifaces1 = new ResourceInterfaces(new List<string>() { ResourceInterfaces.DefaultInterface });
        /// child1.Interface = ifaces1;
        /// try
        /// {
        ///     repr.Children.Add(child1);
        ///     Console.WriteLine("Number of children : {0}", repr.Children.Count);
        ///     Representation firstChild = repr.Children.ElementAt(0);
        /// } catch(Exception ex)
        /// {
        ///     Console.WriteLine("Exception caught : " + ex.Message);
        /// }
        /// </code>
        public ICollection<Representation> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object. Can also dispose any other disposable objects.
        /// </summary>
        /// <param name="disposing">If true, disposes any disposable objects. If false, does not dispose disposable objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free managed objects
                Type?.Dispose();
                Interface?.Dispose();
                Attributes?.Dispose();
                foreach(var child in Children)
                {
                    child.Dispose();
                }
            }

            Interop.IoTConnectivity.Common.Representation.Destroy(_representationHandle);
            _disposed = true;
        }

        private void ChildrenCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Representation r in e.NewItems)
                {
                    int ret = Interop.IoTConnectivity.Common.Representation.AddChild(_representationHandle, r._representationHandle);
                    if (ret != (int)IoTConnectivityError.None)
                    {
                        Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to add child");
                        throw IoTConnectivityErrorFactory.GetException(ret);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Representation r in e.NewItems)
                {
                    int ret = Interop.IoTConnectivity.Common.Representation.RemoveChild(_representationHandle, r._representationHandle);
                    if (ret != (int)IoTConnectivityError.None)
                    {
                        Log.Error(IoTConnectivityErrorFactory.LogTag, "Failed to remove child");
                        throw IoTConnectivityErrorFactory.GetException(ret);
                    }
                }
            }
        }
    }
}
