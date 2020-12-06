// *********************************************************************** Assembly : PathFinder
// Author : xenonsmurf Created : 04-03-2020 Created : 04-03-2020 Created : 04-03-2020 Created :
// Created : 04-03-2020 Created : 04-03-2020 Created : 04-03-2020 Created :
//
// Last Modified By : xenonsmurf Last Modified On : 04-04-2020 Last Modified On : 04-12-2020 Last
// Last Modified On : 07-14-2020 ***********************************************************************
// <copyright file="Imports.cs" company="Xenonsmurf">
//     Copyright © 2020
// </copyright>
// <summary>
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace EasyFarm.ffxi.MeshWalker
{
    /// <summary>
    /// Class FFXINAV. Implements the <see cref="System.IDisposable"/>
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public class FFXINAV : IDisposable
    {
        /// <summary>
        /// Creates the FFXINav class.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "CreateFFXINavClass", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr CreateFFXINavClass();

        /// <summary>
        /// Disposes the FFXINav class.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        [DllImport("FFXINAV.dll", EntryPoint = "DisposeFFXINavClass", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void DisposeFFXINavClass(IntPtr pFFXINavClassObject);

        /// <summary>
        /// Determines whether this instance [can see destination] the specified p ffxi nav class object.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>
        /// <c>true</c> if this instance [can see destination] the specified p ffxi nav class
        /// object; otherwise, <c>false</c>.
        /// </returns>
        [DllImport("FFXINAV.dll", EntryPoint = "CanSeeDestination", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CanSeeDestination(IntPtr pFFXINavClassObject, position_t start, position_t end);

        /// <summary>
        /// The old string
        /// </summary>
        public string oldString = string.Empty;

        /// <summary>
        /// Finds the random path.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="maxRadius">The maximum radius.</param>
        /// <param name="maxTurns">The maximum turns.</param>
        /// <param name="UseCustom">
        /// if set to <c>true</c> [use custom] set true if you are using meshes made with noesis obj files.
        /// </param>
        /// <param name="UseCustom">
        /// if set to <c>false</c> [use custom] set false if using meshes made with FFXINAV using
        /// pathfinder obj files
        /// </param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "FindRandomPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool FindRandomPath(IntPtr pFFXINavClassObject, position_t start, float maxRadius, sbyte maxTurns, bool UseCustom);

        /// <summary>
        /// Unloads meshes
        /// </summary>
        [DllImport("FFXINAV.dll", EntryPoint = "unload", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern bool unload(IntPtr pFFXINavClassObject);

        [DllImport("FFXINAV.dll", EntryPoint = "unloadMeshBuilder", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern bool unloadMeshBuilder(IntPtr pFFXINavClassObject);

        /// <summary>
        /// Initializes the specified p ffxi nav class object.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="LogFileName">Name of the log file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "Initialize", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool initialize(IntPtr pFFXINavClassObject, string LogFileName);

        /// <summary>
        /// Loads the specified p ffxi nav class object.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "load", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool load(IntPtr pFFXINavClassObject, string path);

        /// <summary>
        /// Loads the object file.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "LoadOBJFile", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LoadOBJFile(IntPtr pFFXINavClassObject, string path);

        /// <summary>
        /// Dumps the nav mesh.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "DumpNavMesh", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool DumpNavMesh(IntPtr pFFXINavClassObject, string path);

        /// <summary>
        /// Gets the log message.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <returns>System.String.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "GetLogMessage", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string getLogMessage(IntPtr pFFXINavClassObject);

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="UseCustomNavMesh">
        /// if set to <c>true</c> [use Custom NavMesh] set true if you are using meshes made with
        /// noesis obj files.
        /// </param>
        /// <param name="UseCustomNavMesh">
        /// if set to <c>false</c> [use Custom NavMesh] set false if using meshes made with FFXINAV
        /// using pathfinder obj files
        /// </param>
        [DllImport("FFXINAV.dll", EntryPoint = "FindPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void findPath(IntPtr pFFXINavClassObject, position_t start, position_t end, bool UseCustomNavMesh);

        /// <summary>
        /// Finds the closest path.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="UseCustomNavMesh">
        /// if set to <c>true</c> [use Custom NavMesh] set true if you are using meshes made with
        /// noesis obj files.
        /// </param>
        /// <param name="UseCustomNavMesh">
        /// if set to <c>false</c> [use Custom NavMesh] set false if using meshes made with FFXINAV
        /// using pathfinder obj files
        /// </param>
        [DllImport("FFXINAV.dll", EntryPoint = "FindClosestPath", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void FindClosestPath(IntPtr pFFXINavClassObject, position_t start, position_t end, bool UseCustomNavMesh);

        /// <summary>
        /// Gets the distance to wall.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <returns>System.Double.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "GetDistanceToWall", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern double GetDistanceToWall(IntPtr pFFXINavClassObject, position_t start);

        /// <summary>
        /// Gets the rotation.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>System.SByte.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "GetRotation", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern sbyte GetRotation(IntPtr pFFXINavClassObject, position_t start, position_t end);

        /// <summary>
        /// Determines whether [is nav mesh enabled] [the specified p ffxi nav class object].
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <returns>
        /// <c>true</c> if [is nav mesh enabled] [the specified p ffxi nav class object]; otherwise, <c>false</c>.
        /// </returns>
        [DllImport("FFXINAV.dll", EntryPoint = "isNavMeshEnabled", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool isNavMeshEnabled(IntPtr pFFXINavClassObject);

        /// <summary>
        /// Determines whether [is valid position] [the specified p ffxi nav class object].
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="start">The start.</param>
        /// <param name="UseCustom">if set to <c>true</c> [use custom].</param>
        /// <returns>
        /// <c>true</c> if [is valid position] [the specified p ffxi nav class object]; otherwise, <c>false</c>.
        /// </returns>
        [DllImport("FFXINAV.dll", EntryPoint = "IsValidPosition", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsValidPosition(IntPtr pFFXINavClassObject, position_t start, bool UseCustom);

        /// <summary>
        /// Pathpointses the specified p ffxi nav class object.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("FFXINAV.dll", EntryPoint = "Pathpoints", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int pathpoints(IntPtr pFFXINavClassObject);

        /// <summary>
        /// Navs the mesh settings.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="CellSize">Size of the cell.</param>
        /// <param name="CellHeight">Height of the cell.</param>
        /// <param name="AgentHight">The agent hight.</param>
        /// <param name="AgentRadius">The agent radius.</param>
        /// <param name="MaxClimb">The maximum climb.</param>
        /// <param name="MaxSlope">The maximum slope.</param>
        /// <param name="TileSize">Size of the tile.</param>
        /// <param name="RegionMinSize">Minimum size of the region.</param>
        /// <param name="RegionMergeSize">Size of the region merge.</param>
        /// <param name="EdgeMaxLen">Maximum length of the edge.</param>
        /// <param name="EdgeError">The edge error.</param>
        /// <param name="VertsPP">The verts pp.</param>
        /// <param name="DetailSampDistance">The detail samp distance.</param>
        /// <param name="DetailMaxError">The detail maximum error.</param>
        [DllImport("FFXINAV.dll", EntryPoint = "NavMeshSettings", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void navMeshSettings(IntPtr pFFXINavClassObject, double CellSize, double CellHeight, double AgentHight, double AgentRadius, double MaxClimb,
         double MaxSlope, double TileSize, double RegionMinSize, double RegionMergeSize, double EdgeMaxLen, double EdgeError, double VertsPP,
         double DetailSampDistance, double DetailMaxError);

        /// <summary>
        /// Gets the way points.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="itemsHandle">The items handle.</param>
        /// <param name="itemsHandle2">The items handle2.</param>
        /// <param name="xitems">The xitems.</param>
        /// <param name="zitems">The zitems.</param>
        /// <param name="itemCount">The item count.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe bool Get_WayPoints(IntPtr pFFXINavClassObject, out ItemsSafeHandle itemsHandle, out ItemsSafeHandle itemsHandle2,
            out double* xitems, out double* zitems, out int itemCount);

        /// <summary>
        /// Releases the items.
        /// </summary>
        /// <param name="pFFXINavClassObject">The p ffxi nav class object.</param>
        /// <param name="itemsHandle">The items handle.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("FFXINAV.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool ReleaseItems(IntPtr pFFXINavClassObject, ItemsSafeHandle itemsHandle);


        [DllImport("FFXINAV.dll", EntryPoint = "EnableNearestPoly", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool EnableNearestPoly(IntPtr pFFXINavClassObject, position_t Pos, bool Enable, bool UseCustomMesh);

  
        /// <summary>
        /// Gets the way points wrapper.
        /// </summary>
        /// <param name="xitems">The xitems.</param>
        /// <param name="zitems">The zitems.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns>ItemsSafeHandle.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public unsafe ItemsSafeHandle Get_WayPoints_Wrapper(out double* xitems, out double* zitems, out int itemsCount)
        {
            ItemsSafeHandle itemsHandle;
            if (!Get_WayPoints(this.m_pNativeObject, out itemsHandle, out ItemsSafeHandle itemsHandle2, out xitems, out zitems, out itemsCount))
            {
                throw new InvalidOperationException();
            }
            return (itemsHandle);
        }

        /// <summary>
        /// Class ItemsSafeHandle. Implements the <see cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
        /// </summary>
        /// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
        public class ItemsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ItemsSafeHandle"/> class.
            /// </summary>
            public ItemsSafeHandle()
                : base(true)
            {
            }

            /// <summary>
            /// When overridden in a derived class, executes the code required to free the handle.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the handle is released successfully; otherwise, in the
            /// event of a catastrophic failure, <see langword="false"/>. In this case, it generates
            /// a releaseHandleFailed Managed Debugging Assistant.
            /// </returns>
            protected override bool ReleaseHandle()
            {
                return true;
            }
        }

        /// <summary>
        /// The m p native object
        /// </summary>
        private IntPtr m_pNativeObject;     // Variable to hold the C++ class's this pointer

        /// <summary>
        /// Gets or sets the waypoints.
        /// </summary>
        /// <value>The waypoints.</value>
        public List<position_t> Waypoints { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFXINAV"/> class.
        /// </summary>
        public FFXINAV()
        {
            // We have to Create an instance of this class through an exported function
            this.m_pNativeObject = CreateFFXINavClass();
            Waypoints = new List<position_t>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="bDisposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool bDisposing)
        {
            if (this.m_pNativeObject != IntPtr.Zero)
            {
                // Call the DLL Export to dispose this class
                DisposeFFXINavClass(this.m_pNativeObject);
                this.m_pNativeObject = IntPtr.Zero;
            }

            if (bDisposing)
            {
                // No need to call the finalizer since we've now cleaned up the unmanaged memory
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FFXINAV"/> class.
        /// </summary>
        ~FFXINAV()
        {
            Dispose(false);
        }

        /// <summary>
        /// Determines whether this instance [can we see destination] the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>
        /// <c>true</c> if this instance [can we see destination] the specified start; otherwise, <c>false</c>.
        /// </returns>
        public bool CanWeSeeDestination(position_t start, position_t end)
        {
            return CanSeeDestination(this.m_pNativeObject, start, end);
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        public bool Unload()
        {
            return unload(this.m_pNativeObject);
        }

        public bool UnloadMeshBuilder()
        {
            return unloadMeshBuilder(this.m_pNativeObject);
        }

        /// <summary>
        /// Initializes the specified pathsize.
        /// </summary>
        /// <param name="LogFileName">Name of the log file.</param>
        public bool Initialize(string LogFileName)
        {
            return initialize(this.m_pNativeObject, LogFileName);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Load(string file)
        {
            load(this.m_pNativeObject, file);
        }

        /// <summary>
        /// Loads the ob jfile.
        /// </summary>
        /// <param name="file">The file.</param>
        public void LoadOBJfile(string file)
        {
            Thread.Sleep(2000);
            LoadOBJFile(this.m_pNativeObject, file);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [dumping mesh].
        /// </summary>
        /// <value><c>true</c> if [dumping mesh]; otherwise, <c>false</c>.</value>
        public bool DumpingMesh { get; set; } = false;

        /// <summary>
        /// Dumps the nav mesh.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Dump_NavMesh(string file)
        {
            if (DumpingMesh == false)
            {
                DumpingMesh = true;
                if (UnloadMeshBuilder())
                {
                    if (DumpNavMesh(this.m_pNativeObject, file))
                    {
                        DumpingMesh = false;
                    }
                }
                else
                    DumpingMesh = false;
            }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetErrorMessage()
        {
            return getLogMessage(this.m_pNativeObject).ToString();
        }

        /// <summary>
        /// Finds the path to posi.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="UseCustonNavMeshes">if set to <c>true</c> [use custon nav meshes].</param>
        public void FindPathToPosi(position_t start, position_t end, bool UseCustonNavMeshes)
        {
            //set false if using DSP Nav files or meshes made with PathFinder & FFXINAV.dll
            //set true if using Meshes made with Noesis map data
            findPath(this.m_pNativeObject, start, end, UseCustonNavMeshes);
        }

        /// <summary>
        /// Finds the closest path.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="UseCustonNavMeshes">if set to <c>true</c> [use custon nav meshes].</param>
        public void findClosestPath(position_t start, position_t end, bool UseCustonNavMeshes)
        {
            //set false if using DSP Nav files or meshes made with PathFinder & FFXINAV.dll
            //set true if using Meshes made with Noesis map data
            FindClosestPath(this.m_pNativeObject, start, end, UseCustonNavMeshes);
        }

        /// <summary>
        /// Determines whether [is nav mesh enabled].
        /// </summary>
        /// <returns><c>true</c> if [is nav mesh enabled]; otherwise, <c>false</c>.</returns>
        public bool IsNavMeshEnabled()
        {
            if (isNavMeshEnabled(this.m_pNativeObject) == false)
            {
                return false;
            }
            if (isNavMeshEnabled(this.m_pNativeObject) == true)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Pathes the count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int PathCount()
        {
            return pathpoints(this.m_pNativeObject);
        }

        /// <summary>
        /// Finds the random path.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="maxRadius">The maximum radius.</param>
        /// <param name="maxTurns">The maximum turns.</param>
        /// <param name="UseCustom">if set to <c>true</c> [use custom].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool findRandomPath(position_t start, float maxRadius, sbyte maxTurns, bool UseCustom)
        {
            return FindRandomPath(this.m_pNativeObject, start, maxRadius, maxTurns, UseCustom);
        }

        /// <summary>
        /// Determines whether [is valid position] [the specified start].
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="UseCustom">if set to <c>true</c> [use custom].</param>
        /// <returns><c>true</c> if [is valid position] [the specified start]; otherwise, <c>false</c>.</returns>
        public bool isValidPosition(position_t start, bool UseCustom)
        {
            return IsValidPosition(this.m_pNativeObject, start, UseCustom);
        }

        /// <summary>
        /// Gets the waypoints.
        /// </summary>
        public unsafe void GetWaypoints()
        {
            Waypoints.Clear();
            if (pathpoints(this.m_pNativeObject) > 0)
            {
                double* xitems;
                double* zitems;
                int itemsCount;

                using (Get_WayPoints_Wrapper(out xitems, out zitems, out itemsCount))
                {
                    for (int i = 0; i < itemsCount; i++)
                    {
                        var position = new position_t { X = (float)xitems[i], Z = (float)zitems[i] };
                        Waypoints.Add(position);
                        
                    }
                }
            }
        }
        /// <summary>
        /// Changes the nav mesh settings.
        /// </summary>
        /// <param name="CellSize">Size of the cell.</param>
        /// <param name="CellHeight">Height of the cell.</param>
        /// <param name="AgentHeight">Height of the agent.</param>
        /// <param name="AgentRadius">The agent radius.</param>
        /// <param name="MaxClimb">The maximum climb.</param>
        /// <param name="MaxSlope">The maximum slope.</param>
        /// <param name="TileSize">Size of the tile.</param>
        /// <param name="RegionMinSize">Minimum size of the region.</param>
        /// <param name="RegionMergeSize">Size of the region merge.</param>
        /// <param name="EdgeMaxLen">Maximum length of the edge.</param>
        /// <param name="EdgeError">The edge error.</param>
        /// <param name="VertsPP">The verts pp.</param>
        /// <param name="DetailSampDistance">The detail samp distance.</param>
        /// <param name="DetailMaxError">The detail maximum error.</param>
        public void ChangeNavMeshSettings(double CellSize, double CellHeight, double AgentHeight, double AgentRadius, double MaxClimb,
         double MaxSlope, double TileSize, double RegionMinSize, double RegionMergeSize, double EdgeMaxLen, double EdgeError, double VertsPP,
         double DetailSampDistance, double DetailMaxError)
        {
            navMeshSettings(this.m_pNativeObject, CellSize, CellHeight, AgentHeight, AgentRadius, MaxClimb, MaxSlope, TileSize,
                RegionMinSize, RegionMergeSize, EdgeMaxLen, EdgeError, VertsPP, DetailSampDistance, DetailMaxError);
        }

        /// <summary>
        /// Converts to single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Single.</returns>
        public static float ToSingle(double value)
        {
            return (float)value;
        }

        /// <summary>
        /// Distances to wall.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns>System.Double.</returns>
        public double DistanceToWall(position_t start)
        {
            try
            {
                if (start.X != 0 && start.Z != 0)
                {
                    return GetDistanceToWall(this.m_pNativeObject, start);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Getrotations the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>System.SByte.</returns>
        public sbyte Getrotation(position_t start, position_t end)
        {
            return GetRotation(this.m_pNativeObject, start, end);
        }
        /// <summary>
        /// Enables the or disable nearest poly.
        /// </summary>
        /// <param name="Pos">The position.</param>
        /// <param name="Enable">if set to <c>true</c> [enable].</param>
        /// <param name="UseCustom">if set to <c>true</c> to use meshes made with noesis data [use custom].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> Always Set to false if using my meshes </returns>
      public bool EnableOrDisableNearestPoly(position_t Pos,bool Enable, bool UseCustom)
        {
            return EnableNearestPoly(this.m_pNativeObject, Pos, Enable, UseCustom);
        }
    }
}