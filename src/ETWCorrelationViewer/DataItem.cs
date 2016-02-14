// ***********************************************************************
// Assembly         : ETWCorrelationViewer
// Author           : Ebersbach
// Created          : 02-13-2016
//
// Last Modified By : Ebersbach
// Last Modified On : 02-14-2016
// ***********************************************************************
// <copyright file="DataItem.cs" company="Me">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace ETWCorrelationViewer
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Class DataItem.
    /// </summary>
    /// <seealso cref="ETWCorrelationViewer.Bindable" />
    public class DataItem : Bindable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataItem" /> class.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <exception cref="System.ArgumentException">EndPoint is earlier than startPoint</exception>
        public DataItem(int itemType, DateTime startPoint, DateTime? endPoint = null)
        {
            if (endPoint < startPoint)
            {
                throw new ArgumentException("Endpoint is earlier than startpoint");
            }

            this.Children = new DataItemCollection();
            ((INotifyPropertyChanged)this.Children).PropertyChanged += this.ProjectData_PropertyChanged;
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.ItemType = itemType;
            this.Visible = true;
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public DataItemCollection Children { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>The end point.</value>
        public DateTime? EndPoint
        {
            get { return this.Get<DateTime?>(); }
            set { this.Set(value); }
        }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>The type of the item.</value>
        public int ItemType
        {
            get { return this.Get<int>(); }
            set { this.Set(value); }
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <value>The start point.</value>
        public DateTime? StartPoint
        {
            get { return this.Get<DateTime?>(); }
            set { this.Set(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataItem" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get { return this.Get<bool>(); }
            set { this.Set(value); }
        }

        /// <summary>
        /// Creates the specified item type.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="subItems">The sub items.</param>
        /// <returns>New create instance of <see cref="DataItem"/>.</returns>
        public static DataItem Create(int itemType, DateTime startPoint, DateTime? endPoint = null, params DataItem[] subItems)
        {
            var item = new DataItem(itemType, startPoint, endPoint);
            foreach (var subItem in subItems)
            {
                item.Children.Add(subItem);
            }

            return item;
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="subItems">The sub items.</param>
        /// <returns>This DataItem instance.</returns>
        public DataItem AddItem(int itemType, DateTime startPoint, DateTime? endPoint = null, params DataItem[] subItems)
        {
            DataItem child = Create(itemType, startPoint, endPoint, subItems);
            return this;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ProjectData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void ProjectData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.StartPoint) && (!this.StartPoint.HasValue || this.StartPoint > this.Children.StartPoint))
            {
                this.StartPoint = this.Children.StartPoint;
            }
            else if (e.PropertyName == nameof(this.EndPoint) && (!this.EndPoint.HasValue || this.EndPoint < this.Children.EndPoint))
            {
                this.EndPoint = this.Children.EndPoint;
            }
        }
    }
}