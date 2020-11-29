using EasyFarm.Pathfinding;
using Xunit;
using System.Numerics;

namespace EasyFarm.Tests.Pathfinding
{
    public class NodeTests
    {
        [Fact]
        public void CanCreateNodeFromVector3()
        {
            // Arrange
            Vector3 position = new Vector3(1.0f);

            // Act
            Node node= new Node(position, true);

            // Assert
            Assert.Equal(1,node.gridX);
            Assert.Equal(1, node.gridY);
            Assert.Equal(true, node.walkable);
        }
    }
}
