using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UntitledBallGame.Editor
{
    public class ListViewItemDragger<T> : MouseManipulator 
        where T : class
    {
        #region Init

        private Vector2 _startPos;
        private bool _isActive;
        private readonly ListView _listView;
        private readonly IList<T> _itemsSource;
        private int _initialTargetIndex;
        private int _currentTargetIndex;
        private IList<VisualElement> _elements;

        public ListViewItemDragger(ListView listView, List<T> itemsSource)
        {
            _listView = listView;
            _itemsSource = itemsSource;
            activators.Add(new ManipulatorActivationFilter {button = MouseButton.LeftMouse});
            _isActive = false;
        }

        #endregion

        #region Registrations

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        #endregion

        private void OnMouseDown(MouseDownEvent e)
        {
            if (target.parent != _listView)
            {
                Debug.LogError($"[{nameof(ListViewItemDragger<T>)}] item is not a child of ListView.");
                e.StopImmediatePropagation();
                return;
            }
            
            if (_isActive)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                _startPos = e.localMousePosition;
                _elements = _listView.Children().ToList();
                _initialTargetIndex = _elements.IndexOf(target);
                _currentTargetIndex = _initialTargetIndex;

                _isActive = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent e)
        {
            if (!_isActive || !target.HasMouseCapture())
                return;

            Vector2 diff = e.localMousePosition - _startPos;
            
            var prevTop = target.layout.y; // target.style.top.value.value;
            var currTop = Mathf.Clamp(target.layout.y + diff.y, 0, target.parent.layout.height - target.layout.height);
            target.style.top = currTop;
            var currBottom = target.layout.yMax;

            var dragDirection = DragDirection.None;
            if (currTop < prevTop)
                dragDirection = DragDirection.Up;
            else if (currTop > prevTop)
                dragDirection = DragDirection.Down;

            if (dragDirection == DragDirection.Up && _currentTargetIndex - 1 >= 0)
            {
                var element = _elements[_currentTargetIndex - 1];
                if (currTop < element.layout.center.y)
                {
                    element.style.top = element.style.top.value.value + element.layout.height;
                    Swap(_elements, _currentTargetIndex, _currentTargetIndex - 1);
                    _currentTargetIndex--;
                }
            }
            else if (dragDirection == DragDirection.Down && _currentTargetIndex + 1 < _elements.Count)
            {
                var element = _elements[_currentTargetIndex + 1];
                if (currBottom > element.layout.center.y)
                {
                    element.style.top = element.style.top.value.value - element.layout.height;
                    Swap(_elements, _currentTargetIndex, _currentTargetIndex + 1);
                    _currentTargetIndex++;
                }
            }

            e.StopPropagation();
        }

        private void Swap<TElement>(IList<TElement> list, int firstIdx, int secondIdx)
        {
            var temp = list[firstIdx];
            list[firstIdx] = list[secondIdx];
            list[secondIdx] = temp;
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            if (!_isActive || !target.HasMouseCapture() || !CanStopManipulation(e))
                return;

            target.style.top = target.layout.height * _currentTargetIndex;

            var item = _itemsSource[_initialTargetIndex];
            _listView.RemoveAt(_initialTargetIndex);
            _itemsSource.RemoveAt(_initialTargetIndex);

            _listView.Insert(_currentTargetIndex, target);
            _itemsSource.Insert(_currentTargetIndex, item);

            _listView.Refresh();
            
            _isActive = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
        
        private enum DragDirection
        {
            None,
            Up,
            Down
        }
    }
}