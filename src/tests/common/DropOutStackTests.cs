using System;
using System.Collections.Generic;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Xunit;

namespace Midicontrol.Tests
{
    public class DropOutStackTests
    {
        [Fact]
        public void ShouldReturnLastPushedItem()
        {
            DropOutStack<int> stack = new DropOutStack<int>(1);

            stack += 2;
            stack += 1;

            Assert.Equal(1, stack.Pop());
        }

        [Fact]
        public void ShouldNotExceedDefinedCapacity()
        {
            List<int> values = new(new[] { 2, 3 });
            DropOutStack<int> stack = new DropOutStack<int>(1);

            foreach (int value in values)
            {
                stack += value;
            }

            Assert.Equal(3, stack.Pop());
            Assert.NotEqual(2, stack.Pop());
        }

        [Fact]
        public void ShouldNotConsumeItemOnPeekMethod()
        {
            DropOutStack<int> stack = new DropOutStack<int>(2);
            stack.Push(2);
            stack.Push(3);

            Assert.Equal(3, stack.Peek());
            Assert.Equal(3, stack.Pop());
        }
    }
}