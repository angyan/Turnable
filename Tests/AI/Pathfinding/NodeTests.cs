using Turnable.AI.Pathfinding;
using FluentAssertions;

namespace Tests.AI.Pathfinding
{
    public class NodeTests
    {
        [Fact]
        public void Comparing_nodes_at_same_location()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(1, 2);

            bool areEqual = Node.Equals(node1, node2);

            areEqual.Should().BeTrue();
        }

        [Fact]
        public void Comparing_nodes_at_different_locations()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(2, 3);

            bool areEqual = Node.Equals(node1, node2);

            areEqual.Should().BeFalse();
        }

        [Fact]
        public void Comparing_to_another_node_at_the_same_location()
        {
            var sut = new Node(1, 2);
            var node = new Node(1, 2);

            bool areEqual = sut.Equals(node);

            areEqual.Should().BeTrue();
        }

        [Fact]
        public void Comparing_to_another_node_at_a_different_location()
        {
            var sut = new Node(1, 2);
            var node = new Node(2, 3);

            bool areEqual = sut.Equals(node);

            areEqual.Should().BeFalse();
        }

        [Fact]
        public void Comparing_to_an_object_reference_to_a_node_at_the_same_location()
        {
            var sut = new Node(1, 2);
            object node = new Node(1, 2);

            bool areEqual = sut.Equals(node);

            areEqual.Should().BeTrue();
        }

        [Fact]
        public void Comparing_to_an_object_reference_to_a_node_at_a_different_location()
        {
            var sut = new Node(1, 2);
            object node = new Node(2, 3);

            bool areEqual = sut.Equals(node);

            areEqual.Should().BeFalse();
        }

        [Fact]
        public void Comparing_nodes_at_the_same_location_using_the_equality_operator()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(1, 2);

            bool areEqual = node1 == node2;

            areEqual.Should().BeTrue();
        }

        [Fact]
        public void Comparing_nodes_at_different_locations_using_the_equality_operator()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(2, 3);

            bool areEqual = node1 == node2;

            areEqual.Should().BeFalse();
        }

        [Fact]
        public void Comparing_nodes_at_the_same_location_using_the_inequality_operator()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(1, 2);

            bool areEqual = node1 != node2;

            areEqual.Should().BeFalse();
        }
        
        [Fact]
        public void Comparing_nodes_at_different_locations_using_the_inequality_operator()
        {
            var node1 = new Node(1, 2);
            var node2 = new Node(2, 3);

            bool areEqual = node1 != node2;

            areEqual.Should().BeTrue();
        }

        [Fact]
        public void Has_neighboring_nodes()
        {
            var sut = new Node(1, 2);

            HashSet<Node> neighbors = sut.Neighbors;

            neighbors.Should().NotBeNull();
            neighbors.Count().Should().Be(0);
        }

        [Fact]
        public void Neighboring_nodes_are_only_added_once()
        {
            var sut = new Node(1, 2);
            var neighbor = new Node(2, 3);

            sut.Neighbors.Add(neighbor);
            sut.Neighbors.Add(neighbor);

            sut.Neighbors.Count.Should().Be(1);
        }

        [Fact]
        public void Neighboring_nodes_at_the_same_location_are_only_added_once()
        {
            var sut = new Node(1, 2);
            var neighbor1 = new Node(2, 3);
            var neighbor2 = new Node(2, 3);

            sut.Neighbors.Add(neighbor1);
            sut.Neighbors.Add(neighbor2);

            sut.Neighbors.Count.Should().Be(1);
        }

        [Fact]
        public void Hash_code_is_the_same_as_its_locations_hash_code()
        {
            var sut = new Node(1, 2);

            int hashCode = sut.GetHashCode();

            hashCode.Should().Be(sut.Location.GetHashCode());
        }

        [Fact]
        public void Getting_a_human_readable_representation_when_there_are_no_neighbors()
        {
            var sut = new Node(1, 2);

            string s = sut.ToString();

            s.Should().Be("{ Location: (1, 2), Neighbors: None }");
        }

        [Fact]
        public void Getting_a_human_readable_representation_when_there_are_neighbors()
        {
            var sut = new Node(1, 2);
            var neighbor1 = new Node(2, 3);
            var neighbor2 = new Node(3, 4);
            sut.Neighbors.Add(neighbor1);
            sut.Neighbors.Add(neighbor2);

            string s = sut.ToString();

            s.Should().Be("{ Location: (1, 2), Neighbors: [(2, 3), (3, 4)] }");
        }
    }
}

